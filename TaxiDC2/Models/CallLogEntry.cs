

namespace TaxiDC2.Models
{
	/// <summary>
	/// Zaznam z Call logu telefonu
	/// </summary>
	public class CallLogEntry
	{
		public string PhoneNumber { get; set; }
		public AndroidCallType CallType { get; set; } 
		public DateTime CallDate { get; set; }
		public int Duration { get; set; }
		public string CallerName { get; set; }
	}

	public enum AndroidCallType
	{
		Incoming = 1,           // Příchozí hovor
		Outgoing = 2,           // Odchozí hovor
		Missed = 3,             // Zmeškaný hovor
		Voicemail = 4,          // Hovor do hlasové schránky
		Rejected = 5,           // Odmítnutý hovor
		Blocked = 6,            // Zablokovaný hovor
		AnsweredExternally = 7  // Hovor zodpovězený externě (např. prostřednictvím jiného zařízení)
	}

}
