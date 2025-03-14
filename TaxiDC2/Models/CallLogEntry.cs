namespace TaxiDC2.Models
{
	/// <summary>
	/// Zaznam z Call logu telefonu
	/// </summary>
	public class CallLogEntry
	{
		public string PhoneNumber { get; set; }
		public int CallType { get; set; } 
		public DateTime CallDate { get; set; }
		public int Duration { get; set; }
	}
}
