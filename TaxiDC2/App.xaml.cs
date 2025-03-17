using Plugin.FirebasePushNotification;

namespace TaxiDC2
{
	public partial class App : Application
	{
		public App(IServiceProvider serviceProvider)
		{
			//Register Syncfusion license https://help.syncfusion.com/common/essential-studio/licensing/how-to-generate
			Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(StaticConfig.SyncFusionLic);
			InitializeComponent();

			MainPage = serviceProvider.GetRequiredService<AppShell>();

#if !DEMO
			//Handle notification when app is closed here
			CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
			{
				Debug.WriteLine("Received");

			};

			CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
			{
				var bs = serviceProvider.GetRequiredService<IBussinessState>();
				var ds = serviceProvider.GetRequiredService<IDataService>();
				if (bs != null)
				{
					bs.UpdateDeviceKey(p.Token);
					if (bs.ActiveUserId != null)
					{
						var r = Task.Run(async () => await ds.UpdateDeviceKey(p.Token, bs.ActiveUserId.Value)).Result;
					}
				}
				Debug.WriteLine($"TOKEN : {p.Token}");
			};


			CrossFirebasePushNotification.Current.OnNotificationAction += (s, p) =>
			{
				Debug.WriteLine("Action");

				if (!string.IsNullOrEmpty(p.Identifier))
				{
					Debug.WriteLine($"ActionId: {p.Identifier}");
					foreach (var data in p.Data)
					{
						Debug.WriteLine($"{data.Key} : {data.Value}");
					}

				}

			};

			CrossFirebasePushNotification.Current.OnNotificationDeleted += (s, p) =>
			{

				Debug.WriteLine("Deleted");

			};
#endif
		}
	}
}
