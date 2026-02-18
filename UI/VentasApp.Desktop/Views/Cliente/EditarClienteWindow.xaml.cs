using System.Windows;

namespace VentasApp.Desktop.Views.Cliente
{
    public partial class EditarClienteWindow : Window
    {
        public EditarClienteWindow()
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
            DialogResult = true;
            Close();
        }
    }
}
