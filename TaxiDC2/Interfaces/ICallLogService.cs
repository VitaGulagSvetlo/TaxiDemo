namespace TaxiDC2.Interfaces;

/// <summary>
/// Prace s logem hovoru v telefonu
/// </summary>
public interface ICallLogService
{
	/// <summary>
	/// Nacte log hovoru z telefonu
	///
	/// Musi byt implementovana podle cilove platformy
	/// </summary>
	/// <returns></returns>
	Task<List<CallLogEntry>> GetCallLogEntriesAsync();
}