using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using VentasApp.Application.Interfaces.Repositorios;

namespace VentasApp.Desktop.Views.Ventas
{
    public partial class IngresarCodigoVentaWindow : Window
    {
        public string? CodigoVenta { get; private set; }

        public IngresarCodigoVentaWindow()
        {
            InitializeComponent();
            _ = CargarSiguienteCodigoAsync();
            TxtCodigo.Focus();
        }

        private async System.Threading.Tasks.Task CargarSiguienteCodigoAsync()
        {
            try
            {
                using var scope = App.AppHost!.Services.CreateScope();
                var ventaRepo = scope.ServiceProvider.GetRequiredService<IVentaRepository>();
                var ultimoCodigo = await ventaRepo.ObtenerUltimoCodigoVenta();
                
                if (int.TryParse(ultimoCodigo, out int numero))
                {
                    TxtSiguienteCodigo.Text = (numero + 1).ToString();
                }
                else
                {
                    TxtSiguienteCodigo.Text = "1";
                }
            }
            catch
            {
                TxtSiguienteCodigo.Text = "";
            }
        }

        private void OnCodigoKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OnAceptarClick(sender, e);
            }
        }

        private void OnAceptarClick(object sender, RoutedEventArgs e)
        {
            var codigo = TxtCodigo.Text?.Trim();
            
            if (string.IsNullOrWhiteSpace(codigo))
            {
                MessageBox.Show("Debe ingresar un codigo de venta", "Codigo requerido", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtCodigo.Focus();
                return;
            }

            CodigoVenta = codigo;
            DialogResult = true;
            Close();
        }

        private void OnCancelarClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
