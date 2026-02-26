using System.Windows;

namespace VentasApp.Desktop.Views.Cliente
{
    public partial class AgregarClienteWindow : Window
    {
        public AgregarClienteWindow()
        {
            InitializeComponent();
        }

        private void OnGuardarClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtNombre.Text))
            {
                MessageBox.Show("Complete el Nombre", "Datos incompletos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DialogResult = true;
            Close();
        }
    }
}