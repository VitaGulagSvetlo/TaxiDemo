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
			// Spustíme asynchronnì dialog pro potvrzení opuštìní aplikace
			Device.BeginInvokeOnMainThread(async () =>
			{
				bool exit = await DisplayAlert("Upozornìní", "Opravdu chcete opustit aplikaci?", "Ano", "Ne");
				if (exit)
				{
					// Platformovì specifické ukonèení aplikace – implementujte dle cílové platformy
					System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
					// Nebo napø. pro Android:
#if ANDROID
					Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
#endif
				}
			});
			// Vracíme true, èímž potlaèíme defaultní chování tlaèítka zpìt
			return true;
	}

}