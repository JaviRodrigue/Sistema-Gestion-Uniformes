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

    [ObservableProperty]
    private ObservableCollection<ClienteCardDto> _clientes;

    public ClienteViewModel(VentasApp.Application.Interfaces.Repositorios.IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
        CargarClientes();
    }

    [RelayCommand]
    private void EditarCliente(ClienteCardDto? cliente)
    {
        if (cliente is null) return;
        // TODO: abrir ventana/flujo de edición real
        cliente.Nombre = cliente.Nombre + " (editado)";
        OnPropertyChanged(nameof(Clientes));
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