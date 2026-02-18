using System.Globalization;
using System.Windows;

namespace VentasApp.Desktop.Views.Productos
{
    public partial class EditarProductoWindow : Window
    {
        public EditarProductoWindow()
        {
            InitializeComponent();
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

            DialogResult = true;
            Close();
        }
    }
}
