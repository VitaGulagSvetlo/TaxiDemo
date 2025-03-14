using Android.Provider;
using Application = Android.App.Application;

namespace TaxiDC2.Platforms.Android;

public class CallLogService : ICallLogService
{
	public async Task<List<CallLogEntry>> GetCallLogEntriesAsync()
	{
		var callLogs = new List<CallLogEntry>();

		if (await CheckAndRequestCallLogPermission() == PermissionStatus.Granted)
		{
			var contentResolver = Application.Context.ContentResolver;
			var uri = CallLog.Calls.ContentUri;

			string[] projection = new string[]
			{
				CallLog.Calls.Number,
				CallLog.Calls.Type,
				CallLog.Calls.Date,
				CallLog.Calls.Duration
			};


			// Řazení od nejnovějšího
			var cursor = contentResolver.Query(uri, projection, null, null, CallLog.Calls.Date + " DESC");

			if (cursor != null)
			{
				try
				{
					if (cursor.MoveToFirst())
					{
						int c = 0;
						do
						{
							var number = cursor.GetString(cursor.GetColumnIndex(projection[0]));
							var type = cursor.GetInt(cursor.GetColumnIndex(projection[1]));
							var dateLong = cursor.GetLong(cursor.GetColumnIndex(projection[2]));
							var duration = cursor.GetInt(cursor.GetColumnIndex(projection[3]));

							callLogs.Add(new CallLogEntry
							{
								PhoneNumber = number,
								CallType = type,
								CallDate = DateTimeOffset.FromUnixTimeMilliseconds(dateLong).DateTime,
								Duration = duration
							});
							if (c++>100) break;
						} while (cursor.MoveToNext());
					}
				}
				finally
				{
					cursor.Close();
				}
			}
		}

		return callLogs;
	}


	public async Task<PermissionStatus> CheckAndRequestCallLogPermission()
	{
		PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.Phone>();

		if (status == PermissionStatus.Granted)
			return status;

		if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
		{
			// Prompt the user to turn on in settings
			// On iOS once a permission has been denied it may not be requested again from the application
			return status;
		}

		if (Permissions.ShouldShowRationale<Permissions.Phone>())
		{
			// Prompt the user with additional information as to why the permission is needed
		}

		status = await Permissions.RequestAsync<Permissions.Phone>();

		return status;
	}
}