namespace TaxiDC2.Services;

public class DataService : DataServiceBase
{
	private readonly IBussinessState _bs;

	public DataService(IApiProxy apiProxy, IBussinessState bs) : base(apiProxy)
	{
		_bs = bs;
		apiProxy.ServerUrl = bs.ServerUrl;
	}

	// *******************************************************************************************************
	// nasledujici metody musi byt implementovany v klientskem projektu protoze potrebuji pristup k SHELL !!!!
	// *******************************************************************************************************

	/// <summary>
	/// Vola API s moznosti RETRY
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="apiCall"></param>
	/// <returns></returns>
	protected override async Task<T> ExecuteWithRetry<T>(Func<Task<ServiceState<T>>> apiCall)
	{
		while (true)
		{
			var result = await apiCall();
			if (result.IsSuccess)
			{
				return result.Data;
			}

			// Zobrazíme chybovou hlášku s možností Retry
			bool retry = await Shell.Current.DisplayAlert("Error", result.Message, "Retry", "Cancel");

			if (!retry)
			{
				return default; // Uživatel klikl na Cancel, vracíme null
			}

			Debug.WriteLine("Retrying API call...");
		}
	}

	/// <summary>
	/// Vola API s moznosti RETRY
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="apiCall"></param>
	/// <returns></returns>
	protected override async Task<bool> ExecuteWithRetry(Func<Task<ServiceState>> apiCall)
	{
		while (true)
		{
			var result = await apiCall();
			if (result.IsSuccess)
			{
				return true;
			}

			// Zobrazíme chybovou hlášku s možností Retry
			bool retry = await Shell.Current.DisplayAlert("Error", result.Message, "Retry", "Cancel");

			if (!retry)
			{
				return false; // Uživatel klikl na Cancel
			}

			Debug.WriteLine("Retrying API call...");
		}
	}
}