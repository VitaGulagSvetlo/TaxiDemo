using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using TaxiDC2.Components;
using TaxiDC2.Components.Login;

namespace TaxiDC2.ViewModels
{
	public partial class SignUpViewModel : BaseViewModel
	{
		private readonly FirebaseAuthClient _authClient;
		private readonly IBussinessState _bs;

		#region Properties

		[ObservableProperty]
		[Required(ErrorMessage = "Email je povinný")]
		[EmailAddress(ErrorMessage = "Neplatný formát emailu")]
		private string _email;

		[ObservableProperty]
		[Required(ErrorMessage = "Heslo je povinné")]
		[MinLength(6, ErrorMessage = "Heslo musí mít alespoň 6 znaků")]
		private string _password1;

		[ObservableProperty]
		[Required(ErrorMessage = "Heslo je povinné")]
		[MinLength(6, ErrorMessage = "Heslo musí mít alespoň 6 znaků")]
		private string _password2;

		[ObservableProperty]
		[Required(ErrorMessage = "Jméno je povinné")]
		[MaxLength(50, ErrorMessage = "Jméno musí mít max 50 znaků")]
		private string _firstName;

		[ObservableProperty]
		[Required(ErrorMessage = "Příjmení je povinné")]
		[MaxLength(50, ErrorMessage = "Příjmení musí mít max 50 znaků")]
		private string _lastName;

		[ObservableProperty]
		[Required(ErrorMessage = "Telefon je povinný")]
		[MaxLength(20, ErrorMessage = "Telefon musí mít max 20 znaků")]
		private string _phoneNumber;

		#endregion

		/// <inheritdoc/>
		public SignUpViewModel(IDataService dataService, FirebaseAuthClient authClient, IBussinessState bs) : base(dataService)
		{
			_authClient = authClient;
			_bs = bs;
		}

		[RelayCommand]
		private async Task SignUp()
		{
			Message = string.Empty;

			ValidateAllProperties();
			if (!HasErrors)
			{
				if (Password1 != Password2)
				{
					Message = "Hesla se neshodují";

					return;
				}

				IsBusy = true;
				try
				{
					UserCredential result =
						await _authClient.CreateUserWithEmailAndPasswordAsync(Email, Password1,
							$"{LastName} {FirstName}");
					if (result.User != null)
					{
						var r = await SaveDriverData(_authClient.User.Uid);
						if (r)
						{
							await LoadDriver();
							Shell.Current.FlyoutHeader = new FlyoutHeaderControl(_authClient);
							await Shell.Current.GoToAsync($"/{nameof(SignInPage)}", false);
						}
					}
				}
				catch (FirebaseAuthHttpException ex)
				{
					var errorResponse = JsonSerializer.Deserialize<FirebaseErrorResponse>(ex.ResponseData);
					Message = errorResponse.FirebaseError.Message;
				}
				finally
				{
					IsBusy= false;
				}
			}
		}

		public async Task<bool> SaveDriverData(string key)
		{
			bool ret = await DataService.SaveDriverAsync(new Driver()
			{
				Active = false,
				FirstName = FirstName,
				LastName = LastName,
				PhoneNumber = PhoneNumber,
				Email = Email,
				IsAdmin = false,
				IsDeleted = false,
				NotificationEnable = false,
				MobileDeviceKey = key
			});
			if (ret)
			{
				await Shell.Current.DisplayAlert("Nový uživatel", "Uživatel založen", "OK");
				// OK
				return true;
			}

			return false;
		}

		private async Task LoadDriver()
		{
			Driver[] drl = await DataService.GetDriversAsync(true);
			if (drl != null)
			{
				Driver driver = drl.FirstOrDefault(f => f.MobileDeviceKey == _authClient.User.Uid);
				_bs.ActiveUser = driver;
			}
		}

	}
}
