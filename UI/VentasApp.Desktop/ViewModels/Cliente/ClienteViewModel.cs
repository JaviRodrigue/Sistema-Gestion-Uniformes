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
            new ClienteCardDto { Id = 1, Nombre = "María González", Dni = "30.123.456", Telefonos = "2241-554433", DeudaTotal = 0 },
            new ClienteCardDto { Id = 2, Nombre = "Carlos 'El Moroso' Pérez", Dni = "25.987.654", DeudaTotal = 45200.50m }, // Deudor
            new ClienteCardDto { Id = 3, Nombre = "Escuela N° 1 (Cooperadora)", Dni = "30-11111111-9", DeudaTotal = 0 }
        };
    }
}