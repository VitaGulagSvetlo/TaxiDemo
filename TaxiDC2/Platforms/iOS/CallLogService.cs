

namespace TaxiDC2.Platforms.iOS;

public class CallLogService : ICallLogService
{
	public async Task<List<CallLogEntry>> GetCallLogEntriesAsync()
	{
		var callLogs = new List<CallLogEntry>();

		return callLogs;
	}
}