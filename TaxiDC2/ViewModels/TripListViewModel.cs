using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;

namespace TaxiDC2.ViewModels;

public partial class TripListViewModel : BaseViewModel, IDisposable
{
	private IBussinessState _bs;
	private readonly IPlaySoundService _soundService;

	public ObservableCollection<TripListItemViewModel> Items { get; } = new();

	private Timer timer = null;

	private readonly object _balanceLock = new();

	public TripListViewModel(IBussinessState bs, IPlaySoundService soundService, IDataService dataService) : base(dataService)
	{

		_bs = bs;
		_soundService = soundService;
		ListMode = _bs.TripFilter;

		timer = new Timer(OnTimer, null, TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(15));

		//New trip
		MessagingCenter.Subscribe<INotificationMessage>(this, "MES", (sender) => { Task.Run(async () => await OnMessage(sender)).Wait(); });
	}

	/// <summary>
	/// Prijeti PUSH notifikace a refresh dat
	///
	/// pokud je notifikace udalosti predani na ridice a je to predano na aktualniho ridice
	/// tak zobrazi stranku s alertem
	/// </summary>
	/// <param name="sender"></param>
	/// <returns></returns>
	private async Task OnMessage(INotificationMessage sender)
	{
		await RefreshData();
		NotifyMessageData md = JsonConvert.DeserializeObject<NotifyMessageData>(sender.MessageData);
		System.Diagnostics.Debug.WriteLine($"List page : message ; {md.title} - {md.body}");
		if (md.msg == MessageType.FWD.ToString())
		{
			// Predani jizdy na ridice
			IDictionary<string, Guid> dd = JsonConvert.DeserializeObject<IDictionary<string, Guid>>(md.data);
			if (dd != null && dd["driver"] == _bs.ActiveUserId)
			{
				string t = dd["trip"].ToString();
				// jsem prijemce 
				MainThread.BeginInvokeOnMainThread(async () =>
				{
					await Shell.Current.GoToAsync($"{nameof(TripAlert)}?id={t}&origin=alert");
					_soundService.PlaySystemSound("");
				});
			}
		}
	}

	public void Dispose()
	{
		//unsubscribe Messages
		MessagingCenter.Unsubscribe<INotificationMessage>(this, "MES");
		//clear resources
		timer?.Dispose();
	}

	/// <summary>
	/// Nastavuje list na vsechno 0 nebo privatni { jen moje} 1
	/// </summary>
	[ObservableProperty]
	private bool _listMode;

	public string DriverName => $"{_bs?.ActiveUser?.Inicials ?? "##"}";

	public void OnAppearing()
	{
		IsBusy = true;
	}

	internal async Task RefreshData()
	{
		IsBusy = true;
		try
		{
			IEnumerable<TripListItemViewModel> l = (await DataService.GetTripAsync(true)).Select(TripListItemViewModel.FromTrip);
			if (ListMode) // jen moje
				l = l.Where(w =>
					w.Data.TripState is (TripState.NewOrder or TripState.RejectedByDiver) ||
					w.Data.Driver?.IdDriver == _bs.ActiveUserId);

			// pidan lock, protoze nekdy UI vola refresh vicekrat najednou v ruznejch
			// taskach tak aby se korektne nacetla data do items jen 1x
			lock (_balanceLock)
			{
				Items.Clear();
				foreach (var item in l
							 .OrderBy(o => o.Data.TripState != TripState.NewFromWww)
							 .ThenBy(o => (int)o.Data.TripState > 99)
							 .ThenBy(o => o.MinToDeadLine))
					Items.Add(item);
			}
		}
		catch (Exception ex)
		{
			Debug.WriteLine(ex);
		}
		finally
		{
			IsBusy = false;
		}
	}

	[RelayCommand]
	private async Task LoadItems()
	{
		await RefreshData();
	}

	private void OnTimer(object state)
	{
		Debug.WriteLine($"Refresh time on list");
		foreach (TripListItemViewModel item in Items)
			item.RefreshTime();
	}

	[RelayCommand]
	public async Task TripAdd()
	{
		await Shell.Current.GoToAsync(nameof(NovaJizda));
	}

	[RelayCommand]
	public async Task ProfileOpen()
	{
		//todo : jen ladeni  -- testovaci
		await Shell.Current.GoToAsync($"{nameof(TripAlert)}?id={Items.First().Data.IdTrip}&origin=alert");
		return;
		//todo : jen ladeni  -- testovaci

		if (_bs.IsLogged)
			await Shell.Current.GoToAsync($"{nameof(DetailRidic)}?id={_bs.ActiveUserId}");
	}

	[RelayCommand]
	private async Task Storno(Guid idTrip)
	{
		if (!await Shell.Current.DisplayAlert("STORNO", "Opravdu zrušit jízdu ?", "ANO", "NE"))
		{
			return;
		};

		var ret = await DataService.ChangeTripStateAsync(idTrip, TripState.Canceled);
		if (ret)
		{
			await RefreshData();
		}
		else
		{
			await Shell.Current.DisplayAlert("POZOR", "Jízda nebyla stornována", "OK");
		}
	}

	[RelayCommand]
	private async Task Acc(Guid idTrip)
	{
		if (_bs.ActiveUserId == null)
		{
			await Shell.Current.DisplayAlert("ERROR", "No active driver", "OK");
			return;
		}

		var ret = await DataService.AcceptTripByDriverAsync(idTrip, _bs.ActiveUserId.Value);
		if (ret)
		{
			await RefreshData();
		}
		else
		{
			await Shell.Current.DisplayAlert("POZOR", "Jízda nebyla akceptována", "OK");
		}
	}

	public async void RefreshListMode()
	{
		_bs.TripFilter = ListMode;
		await LoadItems();
	}
}
