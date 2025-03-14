using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TaxiDC2.ViewModels
{
	public partial class TripNewViewModel : BaseViewModel
	{

		private readonly ICallLogService _callLogService;

		[ObservableProperty] private bool _telPickerVisible = false;
		[ObservableProperty] private bool _scPickerVisible = false;
		[ObservableProperty] private bool _adrPickerVisible = false;
		[ObservableProperty] private int _locSelector = 0;
		[ObservableProperty] private int _deadLine = 0;
		[ObservableProperty] private TimeSpan? _casNastupu;
		[ObservableProperty] private string _phone ;

		[ObservableProperty] private Trip _trip = new();


		public ObservableCollection<CallListItem> ListCisel { get; } = new();

		public ObservableCollection<Lokace> ListLokaci { get; } = new();

		public TripNewViewModel(IDataService dataService, ICallLogService callLogService) : base(dataService)
		{
			_callLogService = callLogService;
		}

		internal void NastavAdresu(Lokace sel)
		{
			if (LocSelector == 1)
			{
				Trip.AddressBoarding = sel.Title;
				Trip.AddressBoardingLocX = sel.X;
				Trip.AddressBoardingLocY = sel.Y;
				Trip.AddressBoardingIsValid = true;
			}
			else
			{
				Trip.AddressExit = sel.Title;
				Trip.AddressExitLocX = sel.X;
				Trip.AddressExitLocY = sel.Y;
				Trip.AddressExitIsValid = true;
			}
		}

		/// <summary>
		/// Nastavuje cas nastupu jizdy podle rychlych tlacitek
		/// </summary>
		/// <param name="v"></param>
		[RelayCommand]
		private void DeadlineSet(string v)
		{
			CasNastupu = null;
			if (int.TryParse(v, out int i))
				DeadLine = i;
		}

		/// <summary>
		/// Aktivuje picker pro upresneni adresy
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		[RelayCommand]
		private async Task ValidujAdresu(int v)
		{
			LocSelector = v;
			try
			{
				{
					Lokace[] lo = await DataService.GeocodeAsync(v == 1 ? Trip.AddressBoarding : Trip.AddressExit);
					if (lo == null || !lo.Any())
						return;

					ListLokaci.Clear();
					lo.ToList().ForEach(p => ListLokaci.Add(p));
					OnPropertyChanged(nameof(ListLokaci));

					if (lo.Count() == 1) // pokud je jen jedna adresa tak hned nastav
					{
						NastavAdresu(lo[0]);
					}
					else
						AdrPickerVisible = true;
				}
			}
			catch (Exception es)
			{
				Debug.WriteLine("Chyba nacteni lokaci : " + es.Message);
			}
		}

		/// <summary>
		/// Uklada zadost o jizdu
		/// </summary>
		/// <returns></returns>
		[RelayCommand]
		public async Task SaveData()
		{
			IsBusy = true;
			try
			{
				if (Trip.Customer == null)
					await SetContactFromPhone(Phone);
				if (Trip.Customer != null)
				{
					Trip.Customer.LastAddressBoarding = Trip.AddressBoarding;
					Trip.Customer.LastAddressExit = Trip.AddressExit;
				}

				Trip drv = new()
				{
					OrderTime = DateTime.Now,
					AddressBoarding = string.IsNullOrWhiteSpace(Trip.AddressBoarding) ? "Nezadána" : Trip.AddressBoarding,
					AddressExit = string.IsNullOrWhiteSpace(Trip.AddressExit) ? "Nezadána" : Trip.AddressExit,
					AddressBoardingIsValid = Trip.AddressBoardingIsValid,
					AddressBoardingLocX = Trip.AddressBoardingLocX,
					AddressBoardingLocY = Trip.AddressBoardingLocY,
					AddressExitIsValid = Trip.AddressExitIsValid,
					AddressExitLocX = Trip.AddressExitLocX,
					AddressExitLocY = Trip.AddressExitLocY,

					BoardingTime = VypoctiCas(),
					Driver = null,
					DeadLine = TimeSpan.FromMinutes(DeadLine),
					TripState = TripState.NewOrder,
					Complete = false,
					Memo = Trip.Memo,
					Customer = Trip.Customer
				};

				Trip ret = await DataService.SaveTripAsync(drv);

				if (ret != null)
				{
					// OK prechazim na detailni stranku
					await Shell.Current.GoToAsync($"{nameof(DetailJizda)}?id={ret.IdTrip}");
				}
				else
				{
					//await Shell.Current.DisplayAlert("Ukládání", ret.Message, "OK");
					//ERR
				}
			}
			catch (Exception es)
			{
				Debug.WriteLine("Chyba ukladani jizdy : " + es.Message);
			}
			finally
			{
				IsBusy = false;
			}
		}

		/// <summary>
		/// Akceptuje telefoni cislo z Pickeru
		/// </summary>
		/// <returns></returns>
		[RelayCommand]
		public async Task PhoneAccept()
		{
			await SetContactFromPhone(PhoneSelected);
			TelPickerVisible = false;
		}

		/// <summary>
		/// Storno Pickeru
		/// </summary>
		/// <returns></returns>
		[RelayCommand]
		public async Task PhoneCancel()
		{
			TelPickerVisible = false;
		}

		/// <summary>
		/// vypocita cas startu jizdy
		/// bud podle minut do startu nebo vybraneho specifikovaneho casu
		/// </summary>
		/// <returns></returns>
		private DateTime? VypoctiCas()
		{
			if (CasNastupu == null)
				return DateTime.Now.AddMinutes(DeadLine);


			DateTime den = DateTime.Now.Date;

			if (CasNastupu < DateTime.Now.TimeOfDay)
				den = den.AddDays(1);

			return den + CasNastupu;
		}

		/// <summary>
		/// Aktivuje picker pro posledni telefoni cisla
		/// </summary>
		/// <returns></returns>
		[RelayCommand]
		public async Task CallLogOpen()
		{
			IsBusy = true;
			try
			{
				List<CallLogEntry> log = await _callLogService.GetCallLogEntriesAsync();

				ListCisel.Clear();

				foreach (CallListItem item in ListCisel)
					ListCisel.Add(new CallListItem() { Cislo = item.Cislo, Jmeno = item.Jmeno });

#if DEBUG
				ListCisel.Add(new CallListItem() { Cislo = "111 111 111", Jmeno = "Clovek 1" });
				ListCisel.Add(new CallListItem() { Cislo = "222 222 222", Jmeno = "Clovek 2" });
				ListCisel.Add(new CallListItem() { Cislo = "333 333 333", Jmeno = "Clovek 3" });
				ListCisel.Add(new CallListItem() { Cislo = "444 444 444", Jmeno = "Clovek 4" });
#endif

				OnPropertyChanged(nameof(ListCisel));
			}
			catch (Exception es)
			{
				Debug.WriteLine("Chyba nacteni kontaktu 2: " + es.Message);
			}
			finally
			{
				PhoneSelected = ListCisel.First().Cislo;
				TelPickerVisible = true;
				IsBusy = false;
			}
		}

		/// <summary>
		/// pomocna trida do ktere ukladam jmeno a cislo
		/// </summary>
		public class CallListItem : INotifyPropertyChanged
		{
			public event PropertyChangedEventHandler PropertyChanged;

			public string Cislo { get; set; }
			public string Jmeno { get; set; }
			public bool Missed { get; set; }
			public Color CallColor => Missed ? Color.Parse("DarkRed") : Color.FromHex("#ffbd00");
		}

		/// <summary>
		/// Pokusi se nacist zakaznika podle telefonniho cisla
		/// </summary>
		/// <param name="cislo"></param>
		/// <param name="jmeno"></param>
		/// <returns></returns>
		public async Task SetContactFromPhone(string cislo, string jmeno = "")
		{
			IsBusy = true;
			try
			{
				Customer res = await DataService.GetCustomerByPhoneAsync(cislo);
				if (res != null)
				{
					Trip.Customer = res;
					Phone = res.PhoneNumber;
					PhoneSelected = Phone;
				}
				else
				{
					Trip.Customer = new Customer()
					{
						IdCustomer = Guid.Empty,
						Name = jmeno ?? "",
						PhoneNumber = cislo
					};
					Phone = cislo;
					PhoneSelected = Phone;
				}
			}
			catch (Exception es)
			{
				Debug.WriteLine("Chyba nacteni kontaktu : " + es.Message);
			}
			finally
			{
				IsBusy = false;
			}
		}

		public string CasTxt => VypoctiCas()?.ToShortTimeString();

		public bool CasVisible => CasNastupu != null || DeadLine > 0;

		public string[] ListCisel2 => ListCisel.Select(s => s.Cislo).ToArray();

		public string PhoneSelected { get; set; }

	}

}