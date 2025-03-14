using System.Globalization;

namespace TaxiDC2.Converters;

/// <summary>
/// Konvertuje stav jizdy na textovou reprezentaci
/// </summary>
public class StateToTextConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (!(value is TripState))
		{
			throw new InvalidOperationException("The target must be a TripState");
		}

		switch ((TripState)value)
		{
			case TripState.NewOrder:
				return "Nová objednávka";
			case TripState.AcceptedDiver:
				return "Přijata řidičem";
			case TripState.Running:
				return "Jízda probíhá";
			case TripState.Sms1Sended:
				return "SMS 1 odeslána";
			case TripState.Sms2Sended:
				return "SMS 2 odeslána";
			case TripState.Call:
				return "Volání proběhlo";
			case TripState.ForwardToDiver:
				return "Předání řidiči";
			case TripState.RejectedByDiver:
				return "Zamítnuto řidičem";
			case TripState.Comleted:
				return "Jízda ukončena";
			case TripState.NewFromWww:
				return "Nová objednávka z WWW";
			case TripState.Canceled:
				return "Jízda zrušena";
			default:
				throw new ArgumentOutOfRangeException(nameof(value), value, null);
		}

	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return null;
	}
}