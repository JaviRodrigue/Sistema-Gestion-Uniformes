using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using VentasApp.Desktop.ViewModels.DTOs;
using VentasApp.Domain.Modelo.Cliente;

namespace VentasApp.Desktop.ViewModels.Cliente;

public partial class ClienteViewModel : ObservableObject
{
    // private readonly IGetAllClientesUseCase _getAllClientesUseCase;

    [ObservableProperty]
    private ObservableCollection<ClienteCardDto> _clientes;

    public ClienteViewModel()
    {
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
    private void AgregarCliente()
    {
        var win = new Views.Cliente.AgregarClienteWindow
        {
            Owner = System.Windows.Application.Current.MainWindow
        };
        var ok = win.ShowDialog();
        if (ok == true)
        {
            // TODO: mapear valores y llamar caso de uso CrearCliente
            // Por ahora, agregar mock
            Clientes.Add(new ClienteCardDto
            {
                Id = Clientes.Count + 1,
                Nombre = win.FindName("TxtNombre") is System.Windows.Controls.TextBox t1 ? t1.Text : string.Empty,
                Dni = win.FindName("TxtDni") is System.Windows.Controls.TextBox t2 ? t2.Text : string.Empty,
                Telefonos = win.FindName("TxtTelefonos") is System.Windows.Controls.TextBox t3 ? t3.Text : string.Empty
            });
        }
    }

    private void CargarClientes()
    {
        // Aquí llamarías a _getAllClientesUseCase.Execute();
        Clientes = new ObservableCollection<ClienteCardDto>
        {
            new ClienteCardDto
            {
                Id = 1,
                Nombre = "María González",
                Dni = "30.123.456",
                Telefonos = "2241-554433",
                DeudaTotal = 0,
                UltimasCompras = new ObservableCollection<VentasApp.Desktop.ViewModels.DTOs.VentaResumenDto>
                {
                    new VentasApp.Desktop.ViewModels.DTOs.VentaResumenDto { Id = 1001, Fecha = DateTime.Today.AddDays(-7), EstadoVenta = "Pagada", Total = 15200 },
                    new VentasApp.Desktop.ViewModels.DTOs.VentaResumenDto { Id = 1002, Fecha = DateTime.Today.AddDays(-30), EstadoVenta = "Anulada", Total = 8200 },
                }
            },
            new ClienteCardDto
            {
                Id = 2,
                Nombre = "Carlos 'El Moroso' Pérez",
                Dni = "25.987.654",
                DeudaTotal = 45200.50m, // Deudor
                UltimasCompras = new ObservableCollection<VentasApp.Desktop.ViewModels.DTOs.VentaResumenDto>
                {
                    new VentasApp.Desktop.ViewModels.DTOs.VentaResumenDto { Id = 2001, Fecha = DateTime.Today.AddDays(-3), EstadoVenta = "Pendiente", Total = 12000 },
                    new VentasApp.Desktop.ViewModels.DTOs.VentaResumenDto { Id = 2002, Fecha = DateTime.Today.AddDays(-15), EstadoVenta = "Pagada", Total = 9800 },
                    new VentasApp.Desktop.ViewModels.DTOs.VentaResumenDto { Id = 2003, Fecha = DateTime.Today.AddDays(-20), EstadoVenta = "Pendiente", Total = 23400 },
                }
            },
            new ClienteCardDto
            {
                Id = 3,
                Nombre = "Escuela N° 1 (Cooperadora)",
                Dni = "30-11111111-9",
                DeudaTotal = 0,
                UltimasCompras = new ObservableCollection<VentasApp.Desktop.ViewModels.DTOs.VentaResumenDto>
                {
                    new VentasApp.Desktop.ViewModels.DTOs.VentaResumenDto { Id = 3001, Fecha = DateTime.Today.AddDays(-2), EstadoVenta = "Pagada", Total = 45200 },
                }
            }
        };
    }
}