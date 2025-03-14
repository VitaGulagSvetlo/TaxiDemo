using CommunityToolkit.Maui;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Microsoft.Extensions.Logging;
using Plugin.Maui.Biometric;
using Syncfusion.Maui.Core.Hosting;
using TaxiDC2.Components;
using TaxiDC2.Components.Login;
using TaxiDC2.ViewModels;

namespace TaxiDC2
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.UseMauiCommunityToolkit()
				.ConfigureSyncfusionCore()
				.ConfigureFonts(FAs =>
				{ 
					FAs.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					FAs.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

					FAs.AddFont("FA6-Duotone-Light-300.otf", "FADuotoneLight");
					FAs.AddFont("FA6-Duotone-Regular-400.otf", "FADuotoneRegular");
					FAs.AddFont("FA6-Duotone-Solid-900.otf", "FADuotoneSolid");
					FAs.AddFont("FA6-Duotone-Thin-100.otf", "FADuotoneThin");
					FAs.AddFont("FA6-Pro-Light-300.otf", "FAProLight");
					//FAs.AddFont("FA6-Pro-Regular-400.otf", "FAProRegular");
					FAs.AddFont("FA6-Pro-Regular-400.otf", "FAPro");
					FAs.AddFont("FA6-Pro-Solid-900.otf", "FAProSolid");
					FAs.AddFont("FA6-Pro-Thin-100.otf", "FAProThin");
					FAs.AddFont("FA6-SharpDuotone-Light-300.otf", "FASharpDuoLight");
					FAs.AddFont("FA6-SharpDuotone-Regular-400.otf", "FASharpDuoRegular");
					FAs.AddFont("FA6-SharpDuotone-Solid-900.otf", "FASharpDuoSolid");
					FAs.AddFont("FA6-SharpDuotone-Thin-100.otf", "FASharpDuoThin");
				});

			builder.Services.AddLogging();

			builder.Services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig()
			{
				ApiKey = StaticConfig.FirebaseAuthApiKey,
				AuthDomain = StaticConfig.FirebaseAuthDomain,
				Providers = [new EmailProvider(), new GoogleProvider()],
				UserRepository = new FileUserRepository("Taxi2")
			}));

			// add pages
			builder.Services.AddTransient<AboutPage>();
			builder.Services.AddTransient<MainPage>();
			builder.Services.AddTransient<DetailAuto>();
			builder.Services.AddTransient<DetailJizda>();
			builder.Services.AddTransient<DetailRidic>();
			builder.Services.AddTransient<DetailZakaznik>();
			builder.Services.AddTransient<NovaJizda>();
			builder.Services.AddTransient<SeznamAut>();
			builder.Services.AddTransient<SeznamJizd>();
			builder.Services.AddTransient<SeznamRidicu>();
			builder.Services.AddTransient<SeznamZakazniku>();
			builder.Services.AddTransient<SmsSendView>();
			builder.Services.AddTransient<TripNewViewModel>();
			builder.Services.AddTransient<TripListViewModel>();
			builder.Services.AddTransient<SignInViewModel>();
			builder.Services.AddTransient<SignUpViewModel>();
			builder.Services.AddTransient<LoadingPageViewModel>();

			// viewmodels
			builder.Services.AddTransient<MainViewModel>(); 
			builder.Services.AddTransient<ConfigViewModel>();
			builder.Services.AddTransient<TripDetailViewModel>();
			builder.Services.AddTransient<DriverDetailViewModel>();
			builder.Services.AddTransient<DriverListViewModel>();
			builder.Services.AddTransient<CarDetailViewModel>();
			builder.Services.AddTransient<CarListViewModel>();
			builder.Services.AddTransient<CustomerDetailViewModel>();
			builder.Services.AddTransient<CustomerListViewModel>();
			builder.Services.AddTransient<SmsViewModel>();
			builder.Services.AddTransient<SignInPage>();
			builder.Services.AddTransient<SignUpPage>();
			builder.Services.AddTransient<LoadingPage>();
			builder.Services.AddTransient<TripAlert>();

			builder.Services.AddSingleton<AppShellViewModel>();
			builder.Services.AddSingleton<AppShell>();

			// add services
			builder.Services.AddSingleton<IBussinessState, BussinessState>();
			builder.Services.AddSingleton<IDataService, DataService>();
			builder.Services.AddSingleton<IApiProxy, ApiProxy>();
			builder.Services.AddSingleton<IBiometric>(BiometricAuthenticationService.Default);

#if ANDROID
			builder.Services.AddSingleton<ICallLogService, TaxiDC2.Platforms.Android.CallLogService>();
			builder.Services.AddSingleton<IPlaySoundService, TaxiDC2.Platforms.Android.PlaySoundServiceAndroid>();
#elif IOS
            builder.Services.AddSingleton<ICallLogService, TaxiDC2.Platforms.iOS.CallLogService >();
	        builder.Services.AddSingleton<IPlaySoundService, TaxiDC2.Platforms.iOS.PlaySoundServiceIOS>();
#endif

#if DEBUG
			builder.Logging.AddDebug();
#endif

			return builder.Build();
		}
	}
}
