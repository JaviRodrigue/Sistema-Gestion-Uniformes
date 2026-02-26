using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VentasApp.Application.Interfaces.Repositorios;
using ClienteModel = VentasApp.Domain.Modelo.Cliente.Cliente;

namespace VentasApp.Desktop.Views.Cliente
{
    public partial class ListarClienteVentas : Window
    {
        private readonly IServiceScope _scope;
        private readonly IClienteRepository _clienteRepository;
        private List<ClienteModel> _todosLosClientes = new();

        public string? ClienteNombre { get; private set; }
        public int IdCliente { get; private set; }
        public string? ClienteTelefono { get; private set; }

        public ListarClienteVentas()
        {
            InitializeComponent();
            _scope = App.AppHost!.Services.CreateScope();
            _clienteRepository = _scope.ServiceProvider.GetRequiredService<IClienteRepository>();
            _ = CargarClientesAsync();
        }

        private async System.Threading.Tasks.Task CargarClientesAsync(string? autoSelectNombre = null)
        {
            _todosLosClientes = await _clienteRepository.ListarClientes();
            AplicarFiltro(TxtBuscar?.Text ?? string.Empty, autoSelectNombre);
        }

        private void AplicarFiltro(string filtro, string? autoSelectNombre = null)
        {
            var resultado = string.IsNullOrWhiteSpace(filtro)
                ? _todosLosClientes
                : _todosLosClientes.Where(c => c.Nombre != null &&
                    c.Nombre.Contains(filtro, System.StringComparison.OrdinalIgnoreCase)).ToList();

            ListClientes.ItemsSource = resultado;

            if (autoSelectNombre != null)
            {
                var seleccionado = resultado.FirstOrDefault(c => c.Nombre == autoSelectNombre);
                if (seleccionado != null)
                    ListClientes.SelectedItem = seleccionado;
            }
        }

        private void OnBuscarTextChanged(object sender, TextChangedEventArgs e)
        {
            AplicarFiltro(TxtBuscar.Text);
        }

        private async void OnNuevoClienteClick(object sender, RoutedEventArgs e)
        {
            var win = new AgregarClienteWindow { Owner = this };
            if (win.ShowDialog() != true) return;

            var nombre = (win.FindName("TxtNombre") as TextBox)?.Text?.Trim() ?? string.Empty;
            var dni = (win.FindName("TxtDni") as TextBox)?.Text?.Trim() ?? string.Empty;
            var telefonosStr = (win.FindName("TxtTelefonos") as TextBox)?.Text ?? string.Empty;

            var nuevoCliente = new ClienteModel(nombre, dni);
            var numeros = string.IsNullOrWhiteSpace(telefonosStr)
                ? Enumerable.Empty<string>()
                : telefonosStr.Split(',', ';', '\n')
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s));
            nuevoCliente.ReemplazarTelefonos(numeros);

            await _clienteRepository.Agregar(nuevoCliente);

            await CargarClientesAsync(autoSelectNombre: nombre);
        }

        private void OnCancelarClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OnGuardarClick(object sender, RoutedEventArgs e)
        {
            if (ListClientes.SelectedItem is not ClienteModel seleccionado)
            {
                MessageBox.Show("Debe seleccionar un cliente.", "Sin selecci&#243;n", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ClienteNombre = seleccionado.Nombre;
            IdCliente = seleccionado.Id;
            DialogResult = true;
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            _scope.Dispose();
            base.OnClosed(e);
        }
    }
}



