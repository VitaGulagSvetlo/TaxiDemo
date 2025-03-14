using Android.Media;
using Application = Android.App.Application;


namespace TaxiDC2.Platforms.Android; 

public class PlaySoundServiceAndroid : IPlaySoundService
{
	public void PlaySystemSound(string soundName)
	{
		// Příklad: přehrání výchozího notifikačního zvuku
		var notificationUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
		var ringtone = RingtoneManager.GetRingtone(Application.Context, notificationUri);
		ringtone.Play();

		// Pokud chcete využít parametr soundName, můžete vytvořit mapování na konkrétní zvuky nebo
		// načíst zvuk z resources (např. pomocí MediaPlayer)
	}
}