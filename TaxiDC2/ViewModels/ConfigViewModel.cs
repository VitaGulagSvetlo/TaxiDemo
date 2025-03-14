using System.ComponentModel;
using System.Windows.Input;

namespace TaxiDC2.ViewModels
{
	public class ConfigViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private IBussinessState _bs;
        private IApiProxy _proxy;

        public bool IsBusy { get; set; }
        public string Title { get;  set; }
        public string Message { get;  set; } = string.Empty;
        public string ServerUrl { get;  set; }

        public ICommand SaveDataCmd { get; }
        public ICommand UpdateCmd { get; }


        public ConfigViewModel(IApiProxy proxy, IBussinessState bs)
        {
            _proxy = proxy;
            _bs = bs;
            Title = "Nastavení";
			SaveDataCmd = new Command(async () => await SaveData());
            UpdateCmd = new Command(async () => await Update());
            ServerUrl = _bs.ServerUrl; 

        }

        private async Task Update()
        {
            try
            {
                await Browser.OpenAsync("https://api.advisor-soft.com:8015/update.html", BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                // An unexpected error occured. No browser may be installed on the device.
            }
        }

        public async Task OnAppearing()
        {
            await PingCheck();
        }

        //public void HideKeyboard()
        //{
        //    var context = Android.App.Application.Context;
        //    var inputMethodManager = context.GetSystemService(Context.InputMethodService) as InputMethodManager;
        //    if (inputMethodManager != null && context is Activity)
        //    {
        //        var activity = context as Activity;
        //        var token = activity.CurrentFocus?.WindowToken;
        //        inputMethodManager.HideSoftInputFromWindow(token, HideSoftInputFlags.None);
        //        activity.Window.DecorView.ClearFocus();
        //    }
        //}

        public async Task SaveData()
        {
            _bs.ServerUrl = ServerUrl;
            //HideKeyboard();
            Preferences.Set("serverURL", ServerUrl);
//            DependencyService.Get<IAlertMessage>().ShortAlert("Data uložena");
            await Shell.Current.DisplayAlert("Data uložena", "Data uložena","OK");


			if (await PingCheck())
            {
               // App.Current.MainPage = new AppShell();
                await Shell.Current.GoToAsync($"///SeznamJizd");
            }
        }

        private async Task<bool> PingCheck()
        {
            if (await _proxy.Ping())
            {
                Message = "Server ping OK";
                MessageColor = (Color)Application.Current.Resources["Zelena"];     //nova neprevzata
                PingState = true;
                return true;
            }
            else
            {
                Message = "Server not contacted.";
                MessageColor = (Color)Application.Current.Resources["C13Color"];     //nova neprevzata
                PingState = false;
                return false;
            }

        }

        public bool PingState { get; set; }

        // First time ever launched application
        public bool FirstLaunch => VersionTracking.IsFirstLaunchEver;

        // First time launching current version
        public bool FirstLaunchCurrent => VersionTracking.IsFirstLaunchForCurrentVersion;

        // First time launching current build
        public bool FirstLaunchBuild => VersionTracking.IsFirstLaunchForCurrentBuild;

        // Current app version (2.0.0)
        public string CurrentVersion => VersionTracking.CurrentVersion;

        // Current build (2)
        public string CurrentBuild => VersionTracking.CurrentBuild;

        // Previous app version (1.0.0)
        public string PreviousVersion => VersionTracking.PreviousVersion;

        // Previous app build (1)
        public string PreviousBuild => VersionTracking.PreviousBuild;

        // First version of app installed (1.0.0)
        public string FirstVersion => VersionTracking.FirstInstalledVersion;

        // First build of app installed (1)
        public string FirstBuild => VersionTracking.FirstInstalledBuild;

        // List of versions installed (1.0.0, 2.0.0)
        public IEnumerable<string> VersionHistory => VersionTracking.VersionHistory;

        // List of builds installed (1, 2)
        public IEnumerable<string> BuildHistory => VersionTracking.BuildHistory;

        // Device Model (SMG-950U, iPhone10,6)
        public string Device => DeviceInfo.Model;

        // Manufacturer (Samsung)
        public string Manufacturer => DeviceInfo.Manufacturer;

        // Device Name (Motz's iPhone)
        public string DeviceName => DeviceInfo.Name;

        // Operating System Version Number (7.0)
        public string Version => DeviceInfo.VersionString;

        // Platform (Android)
        public DevicePlatform Platform => DeviceInfo.Platform;

        // Idiom (Phone)
        public DeviceIdiom Idiom => DeviceInfo.Idiom;

        // Device Type (Physical)
        public DeviceType DeviceType => DeviceInfo.DeviceType;

        // Application Name
        public string AppName => AppInfo.Name;

        // Package Name/Application Identifier (com.microsoft.testapp)
        public string PackageName => AppInfo.PackageName;

        // Application Version (1.0.0)
        public string VersionString => AppInfo.VersionString;

        // Application Build Number (1)
        public string BuildString => AppInfo.BuildString;

        public Color MessageColor { get; set; } = Color.Parse("Aqua");
    }
}
