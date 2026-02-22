using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace VentasApp.Desktop.Converters;

public class DeudaColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var tieneDeuda = value is bool b && b;
        var key = tieneDeuda ? "AccentRed" : "SecondaryPastel";
        return System.Windows.Application.Current.Resources[key]
               ?? new SolidColorBrush(tieneDeuda ? Colors.LightCoral : Colors.White);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => Binding.DoNothing;
}
