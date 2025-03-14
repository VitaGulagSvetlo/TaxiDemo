namespace TaxiDC2.Models;

/// <summary>
/// objekt pro posilani dat pres PUSH notifikace
/// </summary>
public class NotifyMessageData
{
    public string body { get; set; }
    public string title { get; set; }
    public string msg { get; set; }
    public string data { get; set; }
}