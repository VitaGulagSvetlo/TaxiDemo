using Android.App;
using Android.Content.PM;
using Android.OS;
using Firebase;
using Plugin.FirebasePushNotification;

namespace TaxiDC2
{
	[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true,
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
							   ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
	public class MainActivity : MauiAppCompatActivity
	{


		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

#if !DEMO
			
			// Inicializace FirebaseApp
			var opt = new FirebaseOptions.Builder();

			opt.SetApiKey(StaticConfig.FirebaseNotifyApiKey);
			opt.SetApplicationId(StaticConfig.FirebaseNotifyApplicationId);
			opt.SetProjectId(StaticConfig.FirebaseNotifyProjectId);
			opt.SetStorageBucket(StaticConfig.FirebaseNotifyStorageBucket);
			opt.SetDatabaseUrl(StaticConfig.FirebaseNotifyDatabaseUrl);

			FirebaseApp.InitializeApp(this, opt.Build());
			FirebasePushNotificationManager.Initialize(this, false);
			
			//Handle notification when app is closed here
			CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
			{
				System.Diagnostics.Debug.WriteLine($"Received : {string.Join(",", p.Data.Values)}");
			};

			CrossFirebasePushNotification.Current.OnNotificationAction += (s, p) =>
			{
				System.Diagnostics.Debug.WriteLine("Action");

				if (!string.IsNullOrEmpty(p.Identifier))
				{
					System.Diagnostics.Debug.WriteLine($"ActionId: {p.Identifier}");
					foreach (var data in p.Data)
					{
						System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
					}

				}

			};
#endif

		}
	}
}
