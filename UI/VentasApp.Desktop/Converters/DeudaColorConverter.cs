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
        // AccentRed (#E57373) si tiene deuda, SecondaryPastel (#E1F5FE) si no
        var colorHex = tieneDeuda ? "#E57373" : "#E1F5FE";
        return new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorHex));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => Binding.DoNothing;
}
