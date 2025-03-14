using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

//using static Android.Webkit.WebStorage;

namespace TaxiDC2.ViewModels
{
	[QueryProperty(nameof(Origin), "origin")]
	public partial class TripDetailViewModel : BaseViewModel
	{
		private readonly IBussinessState _bs;
		public ObservableCollection<Driver> ListRidicu { get; } = new();

		[ObservableProperty] private Trip _trip = new();

		[ObservableProperty] private bool _pickerVisible = false;
		[ObservableProperty] private bool _complete = false;
		[ObservableProperty] private Driver? _selectedDriver ;

		public bool CustomerInfoAvailable => !string.IsNullOrWhiteSpace( Trip.Customer?.Memo);

		public TripDetailViewModel(IDataService dataService, IBussinessState bs) : base(dataService)
		{
			_bs = bs;
		}

		//[DependsOn("TripState")]
		public bool BtnConVisibility => Trip.TripState is (TripState.NewFromWww);

		//[DependsOn("TripState")]
		public bool BtnAccVisibility => IsOwner && Trip.TripState == TripState.ForwardToDiver ||
		                                Trip.TripState is (TripState.NewOrder or TripState.RejectedByDiver);

		//[DependsOn("TripState")]
		public bool BtnCancelVisibility => Trip.TripState is not (TripState.Comleted or TripState.Canceled);

		//[DependsOn("TripState", "Driver")]
		public bool BtnRejectVisibility => IsOwner && Trip.TripState == TripState.ForwardToDiver;

		//[DependsOn("TripState", "Driver")]
		public bool BtnForwardVisibility => Trip.TripState is (TripState.NewOrder or TripState.RejectedByDiver);

		//[DependsOn("TripState", "Driver")]
		public bool BtnSmsVisibility => IsOwner && Trip.TripState is (TripState.AcceptedDiver or TripState.Sms1Sended);

		//[DependsOn("TripState", "Driver")]
		public bool BtnCallVisibility => IsOwner &&
		                                 Trip.TripState is (TripState.AcceptedDiver or TripState.Sms1Sended
			                                 or TripState.Sms2Sended);

		//[DependsOn("TripState", "Driver")]
		public bool BtnRunningVisibility => IsOwner && Trip.TripState is (TripState.AcceptedDiver or TripState.Call
			or TripState.Sms1Sended or TripState.Sms2Sended);

		//[DependsOn("TripState", "Driver")]
		public bool BtnCompleteVisibility => IsOwner && Trip.TripState is (TripState.AcceptedDiver or TripState.Running
			or TripState.Sms1Sended or TripState.Sms2Sended or TripState.Call);

		//[DependsOn("Memo")] public bool IsMemoNotEmpty => !string.IsNullOrWhiteSpace(Trip.Memo);
		public bool CustomerMemoVisible => !string.IsNullOrWhiteSpace(Trip.Customer?.Memo);

		public bool IsOwner => Trip.Driver != null && Trip.Driver.IdDriver == _bs.ActiveUserId;

		/// <summary>
		/// Adresy jsou validni pro hledani trasy
		/// </summary>
		//[DependsOn("AddressBoardingIsValid,AddressExitIsValid")]
		public bool IsValidForRoute => Trip.AddressExitIsValid || Trip.AddressBoardingIsValid;

		//[DependsOn("DeadLine,Counter1")]
		public int MinToDeadLine
		{
			get
			{
				if (Trip.BoardingTime != null) return (int)(Trip.BoardingTime.Value - DateTime.Now).TotalMinutes;
				else
				{
					return
						Trip.DeadLine.HasValue
							? ((Trip.OrderTime + Trip.DeadLine.Value - DateTime.Now).TotalMinutes > 0
								? (int)(Trip.OrderTime + Trip.DeadLine.Value - DateTime.Now).TotalMinutes
								: 0)
							: 0;

				}
			}
		}

		public bool MinLabelVisible { get; set; }

		//[DependsOn("MinToDeadLine")]
		public string MinToDeadLineTxt
		{
			get
			{
				if (MinToDeadLine > 60 * 24)
				{
					MinLabelVisible = false;
					return $"{Trip.BoardingTime:dd.MM. HH:mm}";
				}

				MinLabelVisible = true;
				return MinToDeadLine < 0 ? "0" : MinToDeadLine.ToString();
			}
		}


		//[DependsOn("TripState")]
		public Color StateColor
		{
			get
			{
				Color c = (Color)Application.Current.Resources[$"STC_{Trip.TripState.ToString()}"]; //nova neprevzata
				return c;
			}
		}

		//[DependsOn("TripState")]
		public Color StateTextColor
		{
			get
			{
				Color c = ContrastColor(StateColor);

				return c;
			}
		}

		Color ContrastColor(Color color)
		{
			int d = 0;

			// Counting the perceptive luminance - human eye favors green color...      
			double luminance = (0.299 * color.Red + 0.587 * color.Green + 0.114 * color.Blue) / 255;

			d = luminance > 0.5 ? 0 : 255;

			return Color.FromRgb(d, d, d);
		}

		//[DependsOn("TripState")]
		public FileImageSource StateImage
		{
			get
			{
				FileImageSource i = new();

				switch (Trip.TripState)
				{
					case TripState.NewOrder:
						i = (FileImageSource)ImageSource.FromFile("ico_star_black.png"); // hvezdicka
						break;

					case TripState.RejectedByDiver:
						i = (FileImageSource)ImageSource.FromFile("ico_star_black.png"); // hvezdicka
						break;

					case TripState.ForwardToDiver:
						i = (FileImageSource)ImageSource.FromFile("ico_user_black.png"); // user
						break;

					case TripState.AcceptedDiver:
						i = (FileImageSource)ImageSource.FromFile("ico_user_black.png"); // user
						break;

					case TripState.Running:
						i = (FileImageSource)ImageSource.FromFile("ico_car_black.png"); // auticko
						break;

					case TripState.Sms1Sended:
						i = (FileImageSource)ImageSource.FromFile("ico_sms1_black.png"); // smska
						break;

					case TripState.Sms2Sended:
						i = (FileImageSource)ImageSource.FromFile("ico_sms2_black.png"); // smska
						break;

					case TripState.Call:
						i = (FileImageSource)ImageSource.FromFile("ico_phone_black.png"); // sluchatko
						break;

					case TripState.Comleted:
						i = (FileImageSource)ImageSource.FromFile("ico_thumbsup_black.png"); // palec nahoru
						break;

					case TripState.Canceled:
						i = (FileImageSource)ImageSource.FromFile("ico_cross_black.png"); // krizek
						break;

					case TripState.NewFromWww:
						i = (FileImageSource)ImageSource.FromFile("ico_globe_black.png"); // globus
						break;

					default:
						break;
				}

				return i;
			}
		}

		//[DependsOn("DeadLine,Counter1")]
		public Color TimeColor =>
			Trip.TripState == TripState.Canceled || Trip.TripState == TripState.Comleted
				? (Color)Application.Current.Resources["PozadiTmava"]
				: MinToDeadLine < 2
					? (Color)Application.Current.Resources["Cervena"]
					: MinToDeadLine < 5
						? (Color)Application.Current.Resources["Oranzova"]
						: MinToDeadLine < 60
							? (Color)Application.Current.Resources["Zelena"]
							: (Color)Application.Current.Resources["Zluta2"];

		public async Task LoadData(Guid id)
		{
			var trip = await DataService.GetTripByIdAsync(id);
			Trip = trip??new Trip();
		}

		private async Task PlacePhoneCall(string number)
		{
			if (PhoneDialer.Default.IsSupported)
			{
				try
				{
					PhoneDialer.Open(number);
				}
				catch (ArgumentNullException)
				{
					await Shell.Current.DisplayAlert("POZOR", "Neznámé tel. číslo", "OK");
				}
				catch (Exception ex)
				{
					await Shell.Current.DisplayAlert("", $"Chyba \n{ex.Message}", "OK");
				}
			}
			else
			{
				await Shell.Current.DisplayAlert("", $"Call not supported", "OK");
			}
		}

		[RelayCommand]
		private async Task SmsOpen()
		{
			await Shell.Current.GoToAsync(
				$"{nameof(SmsSendView)}?IdTrip={Trip.IdTrip}&Phone={Uri.EscapeDataString(Trip.Customer.PhoneNumber)}");

			if (Trip.TripState != TripState.Sms1Sended)
			{
				var ret = await DataService.ChangeTripStateAsync(Trip.IdTrip, TripState.Sms1Sended);
				if (ret)
					Trip.TripState = TripState.Sms1Sended;
			}
			else
			{
				var ret = await DataService.ChangeTripStateAsync(Trip.IdTrip, TripState.Sms2Sended);
				if (ret)
					Trip.TripState = TripState.Sms2Sended;
			}
		}

		[RelayCommand]
		private async Task Call()
		{
			await PlacePhoneCall(Trip.Customer?.PhoneNumber);
			var ret = await DataService.ChangeTripStateAsync(Trip.IdTrip, TripState.Call);
			if (ret)
				Trip.TripState = TripState.Call;
		}

		[RelayCommand]
		private async Task Acc()
		{
			if (_bs.ActiveUserId == null)
			{
				await Shell.Current.DisplayAlert("ERROR", "No active driver", "OK");
				return;
			}

			var ret = await DataService.AcceptTripByDriverAsync(Trip.IdTrip, _bs.ActiveUserId.Value);
			if (ret)
			{
				// pokud je zranka volana z alertu tak po akci se vrat na predchozi strtanku
				if (Origin == "alert")
				{
					await Shell.Current.GoToAsync($"..");
					return;
				}

				Trip.TripState = TripState.AcceptedDiver;
				await LoadData(Trip.IdTrip);
			}
			else
			{
				await Shell.Current.DisplayAlert("POZOR", "Jízda nebyla akceptována", "OK");
			}
		}

		[RelayCommand]
		private async Task AcceptWWW()
		{
			if (_bs.ActiveUserId == null)
			{
				await Shell.Current.DisplayAlert("ERROR", "No active driver", "OK");
				return;
			}

			var ret = await DataService.ChangeTripStateAsync(Trip.IdTrip, TripState.NewOrder);
			if (ret)
			{
				try
				{
					string messageText = $"Přijali jsme vaší objednávku z WWW. Děkujeme Taxi-Děčín 777557776 ";
					SmsMessage message = new(messageText, new[] { Trip.Customer.PhoneNumber });
					await Sms.ComposeAsync(message);
				}
				catch (FeatureNotSupportedException ex)
				{
					// Sms is not supported on this device.
				}
				catch (Exception ex)
				{
					// ignored
				}

				Trip.TripState = TripState.NewOrder;
				await LoadData(Trip.IdTrip);
			}
			else
			{
				await Shell.Current.DisplayAlert("POZOR", "Jízda z WWW nebyla akceptována", "OK");
			}
		}

		/// <summary>
		/// Ridic odmitl jizdu
		/// </summary>
		/// <returns></returns>
		[RelayCommand]
		private async Task Rej()

		{
			if (_bs.ActiveUserId == null)
			{
				await Shell.Current.DisplayAlert("ERROR", "No active driver", "OK");
				return;
			}

			var ret = await DataService.RejectTripAsync(Trip.IdTrip, _bs.ActiveUserId.Value);
			if (ret)
			{
				// pokud je zranka volana z alertu tak po akci se vrat na predchozi strtanku
				if (Origin == "alert")
				{
					await Shell.Current.GoToAsync($"..");
					return;
				}

				Trip.TripState = TripState.RejectedByDiver;
				await LoadData(Trip.IdTrip);
			}
			else
			{
				await Shell.Current.DisplayAlert("POZOR", "Jízda nebyla odmítnuta", "OK");
			}
		}

		/// <summary>
		/// Jizda probiha
		/// </summary>
		/// <returns></returns>
		[RelayCommand]
		private async Task Run()
		{
			if (_bs.ActiveUserId == null)
			{
				await Shell.Current.DisplayAlert("ERROR", "No active driver", "OK");
				return;
			}

			var ret = await DataService.ChangeTripStateAsync(Trip.IdTrip, TripState.Running);
			if (ret)
			{
				Trip.TripState = TripState.Running;
				await LoadData(Trip.IdTrip);
			}
			else
			{
				await Shell.Current.DisplayAlert("POZOR", "Jízda nebyla spuštěna", "OK");
			}
		}

		/// <summary>
		/// Zrisit jizdu
		/// </summary>
		/// <returns></returns>
		[RelayCommand]
		private async Task Storno()
		{
			var ret = await DataService.ChangeTripStateAsync(Trip.IdTrip, TripState.Canceled);
			if (ret)
			{
				//Trip.TripState = TripState.Canceled;
				//BindingContext = _viewModel = Task.Run(async () => await LoadData()).Result;
				await Shell.Current.GoToAsync("..");
			}
			else
			{
				await Shell.Current.DisplayAlert("POZOR", "Jízda nebyla stornována", "OK");
			}
		}

		/// <summary>
		/// jizda hohova
		/// </summary>
		/// <returns></returns>
		[RelayCommand]
		private async Task Completed()
		{
			var ret = await DataService.ChangeTripStateAsync(Trip.IdTrip, TripState.Comleted);
			if (ret)
			{
				//Trip.TripState = TripState.Comleted;
				//await Shell.Current.GoToAsync($"///{nameof(TripListPage)}");
				await Shell.Current.GoToAsync("..");
			}
		}

		/// <summary>
		/// Predat na jineho ridice
		/// </summary>
		/// <returns></returns>
		[RelayCommand]
		private async Task Forward()
		{
			IsBusy = true;
			var l = await DataService.GetDriversAsync(true);
			ListRidicu.Clear();
			foreach (var driver in l.OrderBy(o => o.FullName)) ListRidicu.Add(driver);

			PickerVisible = true;
			IsBusy = false;
		}

		[RelayCommand]
		private async Task CustomerEdit()
		{
			if (Trip.Customer != null)
				await Shell.Current.GoToAsync($"{nameof(DetailZakaznik)}?id={Trip.Customer.IdCustomer}");
		}

		[RelayCommand]
		private async Task CustomerInfo()
		{
			if (Trip.Customer != null)
				await Shell.Current.DisplayAlert("Poznámka", Trip.Customer.Memo, "OK");
		}

		[RelayCommand]
		private async Task PickerAccept()
		{
			PickerVisible = false;
			if (SelectedDriver == null) return;
			var ret = await DataService.ForwardTripAsync(Trip.IdTrip, SelectedDriver.IdDriver);
			if (ret)
			{
				await LoadData(Trip.IdTrip);
			}
		}

		[RelayCommand]
		private Task PickerCancel()
		{
			PickerVisible = false;
			return Task.CompletedTask;
		}

		///// <summary>
		///// Odkud jsem sem prisel ?
		///// </summary>
		//private Type OriginPage
		//{
		//	get
		//	{
		//		var navStack = Shell.Current.Navigation.NavigationStack;
		//		if (navStack.Count > 1)
		//		{
		//			var previousPage = navStack[^2];
		//			return previousPage.GetType();
		//		}
		//		return null;
		//	}
		//}

		public string? Origin { get; set; }

	}
}