using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace VentasApp.Desktop.Views.Productos
{
    public partial class AgregarProductoWindow : Window
    {
        private static readonly string[] _talles =
            ["4", "6", "8", "10", "12", "14", "16", "S", "M", "L", "XL"];

        /// <summary>Talle seleccionado; null si la categoría no es Uniforme.</summary>
        public string? TalleSeleccionado { get; private set; }

        public AgregarProductoWindow()
        {
            InitializeComponent();
        }

        private void OnCategoriaChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = CmbCategoria.SelectedItem as ComboBoxItem;
            var esUniforme = item?.Content?.ToString() == "Uniforme";
            PanelTalle.Visibility = esUniforme ? Visibility.Visible : Visibility.Collapsed;

            if (!esUniforme)
                DesmarcarTalles();
        }

        private void DesmarcarTalles()
        {
            foreach (var talle in _talles)
                if (FindName($"Talle{talle}") is RadioButton rb)
                    rb.IsChecked = false;
        }

        private string? ObtenerTalleSeleccionado()
        {
            foreach (var talle in _talles)
                if (FindName($"Talle{talle}") is RadioButton rb && rb.IsChecked == true)
                    return talle;
            return null;
        }

        private void OnGuardarClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtNombre.Text) || CmbCategoria.SelectedItem is null
                || string.IsNullOrWhiteSpace(TxtCosto.Text) || string.IsNullOrWhiteSpace(TxtPrecioVenta.Text))
            {
                MessageBox.Show("Complete Nombre, Categoría, Costo y Precio de Venta", "Datos incompletos",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
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
                MessageBox.Show("Costo y Precio de Venta deben ser mayores a 0", "Validación",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (precioVenta < costo)
            {
                MessageBox.Show("Precio de Venta no puede ser menor que Costo", "Validación",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var esUniforme = (CmbCategoria.SelectedItem as ComboBoxItem)?.Content?.ToString() == "Uniforme";
            if (esUniforme && ObtenerTalleSeleccionado() is null)
            {
                MessageBox.Show("Seleccione un talle para el producto Uniforme.", "Talle requerido",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            TalleSeleccionado = ObtenerTalleSeleccionado();
            DialogResult = true;
            Close();
        }
    }
}