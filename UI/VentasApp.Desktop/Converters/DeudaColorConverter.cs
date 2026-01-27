using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace VentasApp.Desktop.Converters
{
    public class DeudaColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Si es true (tiene deuda), devuelve un color rojizo suave, sino blanco
            if (value is bool tieneDeuda && tieneDeuda)
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFEBEE")); // Rojo muy suave

            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}