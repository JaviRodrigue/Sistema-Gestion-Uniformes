using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Linq;
using System.Collections.Generic;
using VentasApp.Desktop.ViewModels.DTOs;
using VentasApp.Domain.Modelo.Cliente;

namespace VentasApp.Desktop.ViewModels.Cliente;

public partial class ClienteViewModel : ObservableObject
{
    private readonly VentasApp.Application.Interfaces.Repositorios.IClienteRepository _clienteRepository;
    private readonly VentasApp.Application.CasoDeUso.Cliente.ActualizarClienteCasoDeUso _actualizarClienteCasoDeUso;

    [ObservableProperty]
    private ObservableCollection<ClienteCardDto> _clientes;

    public ClienteViewModel(VentasApp.Application.Interfaces.Repositorios.IClienteRepository clienteRepository,
        VentasApp.Application.CasoDeUso.Cliente.ActualizarClienteCasoDeUso actualizarClienteCasoDeUso)
    {
        _clienteRepository = clienteRepository;
        _actualizarClienteCasoDeUso = actualizarClienteCasoDeUso;
        CargarClientes();
    }

    [RelayCommand]
    private async void EditarCliente(ClienteCardDto? cliente)
    {
        if (cliente is null) return;
        var win = new Views.Cliente.EditarClienteWindow
        {
            Owner = System.Windows.Application.Current.MainWindow
        };

        // Prefill fields
        (win.FindName("TxtNombre") as System.Windows.Controls.TextBox)!.Text = cliente.Nombre;
        (win.FindName("TxtDni") as System.Windows.Controls.TextBox)!.Text = cliente.Dni;
        (win.FindName("TxtTelefonos") as System.Windows.Controls.TextBox)!.Text = cliente.Telefonos;

        var ok = win.ShowDialog();
        if (ok == true)
        {
            var nombre = (win.FindName("TxtNombre") as System.Windows.Controls.TextBox)?.Text?.Trim() ?? cliente.Nombre;
            var dni = (win.FindName("TxtDni") as System.Windows.Controls.TextBox)?.Text?.Trim() ?? cliente.Dni;
            var telefonosStr = (win.FindName("TxtTelefonos") as System.Windows.Controls.TextBox)?.Text ?? cliente.Telefonos ?? string.Empty;

            var telefonos = string.IsNullOrWhiteSpace(telefonosStr)
                ? new System.Collections.Generic.List<string>()
                : telefonosStr.Split(',', ';', '\n').Select(s => s.Trim()).Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

            var dto = new VentasApp.Application.DTOs.Cliente.ActualizarClienteDto
            {
                Id = cliente.Id,
                Nombre = nombre,
                Dni = dni,
                Telefonos = telefonos
            };

            await _actualizarClienteCasoDeUso.EjecutarAsync(dto);

            CargarClientes();
        }
    }

    [RelayCommand]
    private void EliminarCliente(ClienteCardDto? cliente)
    {
        if (cliente is null) return;
        if (Clientes.Contains(cliente))
        {
            Clientes.Remove(cliente);
        }
    }

    [RelayCommand]
    private async void AgregarCliente()
    {
        var win = new Views.Cliente.AgregarClienteWindow
        {
            Owner = System.Windows.Application.Current.MainWindow
        };
        var ok = win.ShowDialog();
        if (ok == true)
        {
            var nombre = win.FindName("TxtNombre") is System.Windows.Controls.TextBox t1 ? t1.Text?.Trim() : string.Empty;
            var dni = win.FindName("TxtDni") is System.Windows.Controls.TextBox t2 ? t2.Text?.Trim() : string.Empty;
            var telefonosStr = win.FindName("TxtTelefonos") is System.Windows.Controls.TextBox t3 ? t3.Text : string.Empty;

            var cliente = new VentasApp.Domain.Modelo.Cliente.Cliente(nombre, dni);
            var numeros = string.IsNullOrWhiteSpace(telefonosStr)
                ? Enumerable.Empty<string>()
                : telefonosStr
                    .Split(',', ';', '\n')
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s));

            cliente.ReemplazarTelefonos(numeros);

            await _clienteRepository.Agregar(cliente);

            // Refrescar desde DB para asegurar consistencia
            CargarClientes();
        }
    }

    private async void CargarClientes()
    {
        var list = await _clienteRepository.ListarClientes();
        var mapped = list.Select(c => new ClienteCardDto
        {
            Id = c.Id,
            Nombre = c.Nombre ?? string.Empty,
            Dni = c.DNI ?? string.Empty,
            Telefonos = string.Join(" / ", (c.Telefonos ?? new List<VentasApp.Domain.Modelo.Cliente.Telefono>()).Select(t => t.Numero)),
            DeudaTotal = 0, // TODO: map from ventas/pagos
            UltimasCompras = new ObservableCollection<VentasApp.Desktop.ViewModels.DTOs.VentaResumenDto>()
        });

        Clientes = new ObservableCollection<ClienteCardDto>(mapped);
    }
}