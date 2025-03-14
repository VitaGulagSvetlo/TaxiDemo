
using AudioToolbox;

namespace TaxiDC2.Platforms.iOS;

public class PlaySoundServiceIOS : IPlaySoundService
{
	public void PlaySystemSound(string soundName)
	{
		// Příklad: přehrání systémového zvuku pomocí systémového ID.
		// Pro ilustraci použijeme systémové ID 1007, které odpovídá jednomu z vestavěných zvuků.
		// V praxi můžete vytvořit mapování dle hodnoty soundName.
		uint systemSoundId = 1007;
		SystemSound systemSound = new SystemSound(systemSoundId);
		systemSound.PlaySystemSound();
	}
}
