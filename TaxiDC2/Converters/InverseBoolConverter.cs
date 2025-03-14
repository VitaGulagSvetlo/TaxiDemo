using System.Globalization;

namespace TaxiDC2.Converters;

/// <summary>
/// Invetuje boolean hodnotu v bindingu
/// </summary>
public class InverseBoolConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (!(value is bool))
		{
			throw new InvalidOperationException("The target must be a boolean");
		}

		return !(bool)value;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return null;
	}
}