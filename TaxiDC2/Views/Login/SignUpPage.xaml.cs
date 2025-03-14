using TaxiDC2.ViewModels;

namespace TaxiDC2.Components.Login;

public partial class SignUpPage : ContentPage
{
	private readonly SignUpViewModel _model;

	public SignUpPage(SignUpViewModel signInViewModel)
	{
		InitializeComponent();
		BindingContext = _model = signInViewModel;
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