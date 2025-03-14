using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using TaxiDC2.Components.Login;

namespace TaxiDC2.ViewModels
{
	public partial class AppShellViewModel:BaseViewModel, IDisposable
	{
		private readonly FirebaseAuthClient _authClient;
		private readonly CancellationTokenSource _cancellationTokenSource = new();

		public AppShellViewModel(IDataService dataService, FirebaseAuthClient authClient):base(dataService)
		{
			_authClient = authClient;

			// Spustíme periodickou kontrolu serveru
			StartServerCheck();
		}

		public bool IsServerOk { get; set; }

		public bool IsSigned => !string.IsNullOrWhiteSpace(_authClient?.User?.Info?.Email);

		public Color ServerColor => IsServerOk ? Color.FromArgb("#ff141414"):Colors.DarkRed;

		[RelayCommand]
		async Task Logout()
		{
			bool ansver = await Shell.Current.DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
			if(ansver)
			{
				_authClient.SignOut();
				await Shell.Current.GoToAsync($"{nameof(SignInPage)}");
			}
		}

		private async void StartServerCheck()
		{
			while (!_cancellationTokenSource.IsCancellationRequested)
			{
				try
				{
					// Provede asynchroní kontrolu serveru
					bool pingResult = await DataService.PingAsync();
					IsServerOk = pingResult;
				}
				catch
				{
					// Pokud dojde k chybě, považujeme server za nedostupný
					IsServerOk = false;
				}

				// Počká 10 sekund před dalším pingem
				await Task.Delay(10000, _cancellationTokenSource.Token);
			}
		}

		// Uvolníme zdroje při dispose
		public void Dispose()
		{
			_cancellationTokenSource.Cancel();
		}
	}
}
