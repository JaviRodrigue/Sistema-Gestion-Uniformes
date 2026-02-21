using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace VentasApp.Desktop.Views.Productos
{
    public partial class EditarProductoWindow : Window
    {
        private static readonly string[] _talles =
            ["SinTalle", "16", "18", "20", "22", "24", "26", "28", "30", "32", "XS", "S", "M", "L", "XL"];

        public string? TalleSeleccionado { get; private set; }

        public EditarProductoWindow()
        {
            InitializeComponent();
        }

        public void SetTalle(string? talle)
        {
            var key = talle is null ? "SinTalle" : talle;
            if (FindName($"Talle{key}") is RadioButton rb)
                rb.IsChecked = true;
        }

        private string? ObtenerTalleSeleccionado()
        {
            foreach (var talle in _talles)
                if (FindName($"Talle{talle}") is RadioButton rb && rb.IsChecked == true)
                    return talle == "SinTalle" ? null : talle;
            return null;
        }

        private void OnCancelarClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OnGuardarClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtNombre.Text) || string.IsNullOrWhiteSpace(TxtCosto.Text) || string.IsNullOrWhiteSpace(TxtPrecioVenta.Text))
            {
                MessageBox.Show("Complete Nombre, Costo y Precio de Venta", "Datos incompletos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(TxtCosto.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out var costo))
            {
                MessageBox.Show("Costo inválido", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!decimal.TryParse(TxtPrecioVenta.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out var precioVenta))
            {
                MessageBox.Show("Precio de Venta inválido", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (precioVenta <= 0 || costo <= 0)
            {
                MessageBox.Show("Costo y Precio de Venta deben ser mayores a 0", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (precioVenta < costo)
            {
                MessageBox.Show("Precio de Venta no puede ser menor que Costo", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            TalleSeleccionado = ObtenerTalleSeleccionado();
            DialogResult = true;
            Close();
        }
    }
}
