namespace TaxiDC2.Interfaces;

/// <summary>
/// Drzi stavove informace pro celou aplikaci
/// </summary>
public interface IBussinessState
{
	string DeviceKey { get; set; }
	Driver? ActiveUser { get; set; }
	Guid? ActiveUserId { get; }
	public bool IsLogged { get; }
	public bool IsActive { get; }
	public bool IsAdmin { get; }
	public bool IsCarAssigned { get; }

	string ServerUrl { get; set; }
	public bool TripFilter { get; set; }
	/// <summary>
	/// Firebase user
	/// </summary>
	public string AuthUserName { get; }

	void UpdateDeviceKey(string eToken);
}