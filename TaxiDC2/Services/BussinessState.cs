using Firebase.Auth;

namespace TaxiDC2.Services
{
	public class BussinessState : IBussinessState
	{
		private readonly FirebaseAuthClient _authClient;

		public BussinessState(FirebaseAuthClient authClient)
		{
			_authClient = authClient;
		}

		public string AuthUserName => _authClient?.User?.Info?.DisplayName;
		public bool IsCarAssigned => ActiveUser?.Car != null;

		private static string _deviceKey;

		/// <summary>
		/// Klic zarizeni
		/// </summary>
		public string DeviceKey { get => _deviceKey; set => _deviceKey = value; }

		/// <summary>
		/// Je prihlasen uzivatel ?
		/// </summary>
		public bool IsLogged => ActiveUser != null;

		// je uzivatel povolen ?
		public bool IsActive => ActiveUser?.Active ?? false;

		public Driver? ActiveUser { get; set; }

		/// <summary>
		/// ID aktualniho usera
		/// </summary>
		public Guid? ActiveUserId => ActiveUser?.IdDriver;

		/// <summary>
		/// je aktualni user adminem ?
		/// </summary>
		public bool IsAdmin => ActiveUser?.IsAdmin ?? false;

		/// <summary>
		/// Bazova adresa API
		/// </summary>
		public string ServerUrl
		{
			get => Preferences.Get("ServerUrl", StaticConfig.DeafaultApiUrl);
			set => Preferences.Set("ServerUrl", value);
		}

		/// <summary>
		/// Nastaveni filtru cest
		/// </summary>
		public bool TripFilter
		{
			get => Preferences.Get("TripFilter", false);
			set => Preferences.Set("TripFilter", value);
		}

		/// <summary>
		/// Aktualizace klice zarizeni pokud se zmeni
		/// </summary>
		/// <param name="eToken"></param>
		public void UpdateDeviceKey(string eToken)
		{
			_deviceKey = eToken;
		}
	}
}