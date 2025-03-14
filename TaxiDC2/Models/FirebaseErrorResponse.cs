using System.Text.Json.Serialization;

namespace TaxiDC2.Models;

public class FirebaseErrorResponse
{
	[JsonPropertyName("error")]
	public FirebaseErrorInfo FirebaseError { get; set; }
}


public class FirebaseErrorInfo
{
	[JsonPropertyName("code")]
	public int Code { get; set; }

	[JsonPropertyName("message")]
	public string Message { get; set; }

	[JsonPropertyName("errors")]
	public List<FirebaseErrorDetail> Errors { get; set; }
}

public class FirebaseErrorDetail
{
	[JsonPropertyName("message")]
	public string Message { get; set; }

	[JsonPropertyName("domain")]
	public string Domain { get; set; }

	[JsonPropertyName("reason")]
	public string Reason { get; set; }
}