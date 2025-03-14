using TaxiDC2.Components.Login;
using TaxiDC2.ViewModels;

namespace TaxiDC2
{
	public partial class AppShell : Shell
	{
		private readonly AppShellViewModel _model;

		public AppShell(AppShellViewModel model)
		{
			InitializeComponent();
			BindingContext = _model = model;

			RegisterAllRoutes();
		}

		/// <summary>
		/// Registruje routy pro navigaci
		/// </summary>
		private void RegisterAllRoutes()
		{
			Routing.RegisterRoute(nameof(DetailAuto), typeof(DetailAuto));
			Routing.RegisterRoute(nameof(DetailJizda), typeof(DetailJizda));
			Routing.RegisterRoute(nameof(DetailRidic), typeof(DetailRidic));
			Routing.RegisterRoute(nameof(DetailZakaznik), typeof(DetailZakaznik));
			Routing.RegisterRoute(nameof(NovaJizda), typeof(NovaJizda));
			Routing.RegisterRoute(nameof(SeznamAut), typeof(SeznamAut));
			Routing.RegisterRoute(nameof(SeznamRidicu), typeof(SeznamRidicu));
			Routing.RegisterRoute(nameof(SeznamZakazniku), typeof(SeznamZakazniku));
			Routing.RegisterRoute(nameof(SeznamJizd), typeof(SeznamJizd));
			Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
			Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
			Routing.RegisterRoute(nameof(TripAlert), typeof(TripAlert));
			Routing.RegisterRoute(nameof(SmsSendView), typeof(SmsSendView));
			Routing.RegisterRoute(nameof(SignInPage), typeof(SignInPage));
			Routing.RegisterRoute(nameof(SignUpPage), typeof(SignUpPage));

		}


		protected override bool OnBackButtonPressed()
		{
			// Pokud je pouze aktuální stránka (nebo žádná) v navigačním zásobníku
			if (Navigation.NavigationStack.Count <= 1)
			{
				// Spustíme asynchronně dialog pro potvrzení opuštění aplikace
				Device.BeginInvokeOnMainThread(async () =>
				{
					bool exit = await DisplayAlert("Upozornění", "Opravdu chcete opustit aplikaci?", "Ano", "Ne");
					if (exit)
					{
						// Platformově specifické ukončení aplikace – implementujte dle cílové platformy
						System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
						// Nebo např. pro Android:
#if ANDROID
						Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
#endif
					}
				});
				// Vracíme true, čímž potlačíme defaultní chování tlačítka zpět
				return true;
			}
			return base.OnBackButtonPressed();
		}

	}
}
