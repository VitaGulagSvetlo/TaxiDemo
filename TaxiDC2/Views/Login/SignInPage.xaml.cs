using TaxiDC2.ViewModels;

namespace TaxiDC2.Components.Login;

public partial class SignInPage : ContentPage
{
	private readonly SignInViewModel _model;

	public SignInPage(SignInViewModel model)
	{
		InitializeComponent();
		BindingContext = _model = model;
	}

	protected override bool OnBackButtonPressed()
	{
			// Spust�me asynchronn� dialog pro potvrzen� opu�t�n� aplikace
			Device.BeginInvokeOnMainThread(async () =>
			{
				bool exit = await DisplayAlert("Upozorn�n�", "Opravdu chcete opustit aplikaci?", "Ano", "Ne");
				if (exit)
				{
					// Platformov� specifick� ukon�en� aplikace � implementujte dle c�lov� platformy
					System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
					// Nebo nap�. pro Android:
#if ANDROID
					Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
#endif
				}
			});
			// Vrac�me true, ��m� potla��me defaultn� chov�n� tla��tka zp�t
			return true;
	}

}