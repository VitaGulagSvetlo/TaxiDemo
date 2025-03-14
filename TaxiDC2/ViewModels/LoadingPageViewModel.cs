using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using Plugin.Maui.Biometric;
using TaxiDC2.Components;
using TaxiDC2.Components.Login;

namespace TaxiDC2.ViewModels
{
	public partial class LoadingPageViewModel : BaseViewModel
	{
		private readonly IDataService _dataService;
		private readonly FirebaseAuthClient _authClient;
		private readonly IBiometric _biometricService;
		private readonly IBussinessState _bs;

		public LoadingPageViewModel(IDataService dataService,
			FirebaseAuthClient authClient,
			IBiometric biometricService,
			IBussinessState bs
			) : base(dataService)
		{
			_dataService = dataService;
			_authClient = authClient;
			_biometricService = biometricService;
			_bs = bs;
			CheckUser();
			
		}

		private async void CheckUser()
		{
			IsBusy= true;
			Message = "";
			if (string.IsNullOrWhiteSpace(_authClient?.User?.Info?.Email))
			{
				if (DeviceInfo.Platform == DevicePlatform.WinUI)
				{
					Shell.Current.Dispatcher.Dispatch(async () =>
					{
						await Shell.Current.GoToAsync($"/{nameof(SignInPage)}");
					});
				}
				else
				{
					await Shell.Current.GoToAsync($"/{nameof(SignInPage)}");
				}
			}
			else
			{
				//todo: debug
				await LoadDriver();
				if (!_bs.IsLogged)
				{
					Message = "Uživatel nenalezen v DB nebo problém s připojením k serveru! Kontaktujte admina.";
					IsBusy = false;
					return;
				}
				if (!_bs.IsActive)
				{
					Message = "Uživatel je neaktivní! Kontaktujte admina.";
					IsBusy = false;
					return;
				}
				Shell.Current.FlyoutHeader = new FlyoutHeaderControl(_authClient);
				await Shell.Current.GoToAsync($"///{nameof(MainPage)}");
				return;
				//todo: debug

				BiometricHwStatus biometricStatus = await _biometricService.GetAuthenticationStatusAsync();
				// Handle biometricStatus based on the application's logic

				if (biometricStatus != BiometricHwStatus.Success)
				{
					await Shell.Current.DisplayAlert("Error", "biometricStatus is false", "Close");
					Message = "Biometric authentication is not available on this device";
					IsBusy = false;
					return;
				}

				AuthenticationRequest authenticationRequest = new()
				{
					AllowPasswordAuth = true, // A chance to fallback to password auth
					Title = "Authenticate", // On iOS only the title is relevant, everything else is unused. 
					Subtitle = "Please authenticate using your biometric data",
					//NegativeText = "Use Password", // if AllowPasswordAuth is set to true don't use this it will throw an exception on Android
					Description = "Biometric authentication is required for access",
					AuthStrength = AuthenticatorStrength.Strong // Only relevant on Android
				};

				//tady je to v dispatcheru, protoze jinak to bezelo v jinem tasku nez UI
				//a vyhazovalo to nesmyslne chyby v Android java kodu
				// rozchozeno nahodou a nevim co to presne dela :-)
				Shell.Current.Dispatcher.Dispatch(async () =>
				{
					AuthenticationResponse authenticationResponse = await _biometricService.AuthenticateAsync(authenticationRequest, CancellationToken.None);
					if (authenticationResponse.Status != BiometricResponseStatus.Success)
					{
						await Shell.Current.DisplayAlert("Error", authenticationResponse.ErrorMsg, "Close");
						Message = $"Biometric authentication failed. {authenticationResponse.ErrorMsg}";
						IsBusy = false;
						return;
					}
					else
					{
						await LoadDriver();
						if (!_bs.IsLogged)
						{
							Message = "Uživatel nenalezen v DB nebo problém s připojením k serveru! Kontaktujte admina.";
							IsBusy = false;
							return;
						}

						if (!_bs.IsActive)
						{
							Message = "Uživatel je neaktivní! Kontaktujte admina.";
							IsBusy = false;
							return;
						}
						Shell.Current.FlyoutHeader = new FlyoutHeaderControl(_authClient);
						await Shell.Current.GoToAsync($"///{nameof(MainPage)}");
					}
				});

			}
		}

		private async Task LoadDriver()
		{
			Driver[] drl = await _dataService.GetDriversAsync(true);
			if (drl != null)
			{
				Driver driver = drl.FirstOrDefault(f => f.MobileDeviceKey == _authClient.User.Uid);
				if (driver != null)
				{
					_bs.ActiveUser = driver;
					return;
				}
			}
		}

		[RelayCommand]
		private async Task SignIn()
		{
			await Shell.Current.GoToAsync($"/{nameof(SignInPage)}");
		}
	}
}
