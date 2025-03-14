using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using TaxiDC2.Components;
using TaxiDC2.Components.Login;

namespace TaxiDC2.ViewModels
{
	public partial class SignInViewModel : BaseViewModel
	{
		private readonly FirebaseAuthClient _authClient;
		private readonly IBussinessState _bs;

		[ObservableProperty] private string _email = "peno@penodc.com";
		[ObservableProperty] private string _password = "penox22";

		public bool ServerOK
		{
			get
			{
				bool r = Task.Run(async () => await DataService.PingAsync()).Result;
				return r;
			}
		}

		[RelayCommand]
		private async Task SignUp()
		{
			await Shell.Current.GoToAsync($"/{nameof(SignUpPage)}", true);
		}

		[RelayCommand]
		private async Task Settings()
		{
			await Shell.Current.GoToAsync($"/{nameof(AboutPage)}", true);
		}

		/// <inheritdoc/>
		public SignInViewModel(IDataService dataService, FirebaseAuthClient authClient, IBussinessState bs) : base(dataService)
		{
			_authClient = authClient;
			_bs = bs;
		}

		[RelayCommand]
		private async Task SignIn()
		{
			Message = string.Empty;

			if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
			{
				Message = "Email and password are required !";
				return;
			}

			try
			{
				IsBusy = true;
				UserCredential result = await _authClient.SignInWithEmailAndPasswordAsync(Email, Password);
				if (result.User != null)
				{
					await LoadDriver();
					if (!_bs.IsLogged)
					{
						Message = "Uživatel nenalezen v DB! Kontaktujte admina.";
						return;
					}

					if (!_bs.IsActive)
					{
						Message = "Uživatel není aktivní! Kontaktujte admina.";
						return;
					}

					Shell.Current.FlyoutHeader = new FlyoutHeaderControl(_authClient);
					await Shell.Current.GoToAsync($"///{nameof(MainPage)}", false);
				}
			}
			catch (FirebaseAuthHttpException ex)
			{
				var errorResponse = JsonSerializer.Deserialize<FirebaseErrorResponse>(ex.ResponseData);
				Message = errorResponse.FirebaseError.Message;
			}
			finally
			{
				IsBusy = false;
			}

		}

		private async Task LoadDriver()
		{
			Driver[] drl = await DataService.GetDriversAsync(true);
			Driver driver = drl.FirstOrDefault(f => f.MobileDeviceKey == _authClient.User.Uid);
			_bs.ActiveUser = driver;
		}

	}
}
