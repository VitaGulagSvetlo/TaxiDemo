using System.Collections;
using System.Globalization;

namespace TaxiDC2.Converters;

/// <summary>
/// Vraci prvni chybu z kolekce chyb
/// </summary>
public class FirstErrorConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is IEnumerable errors)
		{
			foreach (var error in errors)
			{
				if (error != null)
				{
					return error.ToString();
				}
			}
		}
		return string.Empty;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}