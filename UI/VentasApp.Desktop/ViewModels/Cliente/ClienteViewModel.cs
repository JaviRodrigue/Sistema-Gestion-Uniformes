using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using VentasApp.Desktop.ViewModels.DTOs;
using VentasApp.Domain.Modelo.Cliente;
using VentasApp.Application.CasoDeUso.Venta;
using VentasApp.Application.CasoDeUso.DetalleVenta;

namespace VentasApp.Desktop.ViewModels.Cliente;

public partial class ClienteViewModel : ObservableObject
{
    private readonly VentasApp.Application.Interfaces.Repositorios.IClienteRepository _clienteRepository;
    private readonly VentasApp.Application.CasoDeUso.Cliente.ActualizarClienteCasoDeUso _actualizarClienteCasoDeUso;
    private readonly ObtenerVentaUseCase _obtenerVenta;
    private readonly VentasApp.Application.CasoDeUso.DetalleVenta.GuardarDetalleVentaUseCase _guardarDetalle;

    [ObservableProperty]
    private ObservableCollection<ClienteCardDto> _clientes;

    public ClienteViewModel(
        VentasApp.Application.Interfaces.Repositorios.IClienteRepository clienteRepository,
        VentasApp.Application.CasoDeUso.Cliente.ActualizarClienteCasoDeUso actualizarClienteCasoDeUso,
        ObtenerVentaUseCase obtenerVenta,
        VentasApp.Application.CasoDeUso.DetalleVenta.GuardarDetalleVentaUseCase guardarDetalle)
    {
        _clienteRepository = clienteRepository;
        _actualizarClienteCasoDeUso = actualizarClienteCasoDeUso;
        _obtenerVenta = obtenerVenta;
        _guardarDetalle = guardarDetalle;
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
    private async Task VerDetalleVenta(object? parameter)
    {
        if (parameter is not VentasApp.Desktop.ViewModels.DTOs.VentaResumenDto venta) return;

        try
        {
            var detalle = await _obtenerVenta.EjecutarAsync(venta.Id);
            if (detalle is null) return;

            var uiDetalle = MapVentaDetalle(detalle);
            uiDetalle.Id = detalle.Id;

            var win = new VentasApp.Desktop.Views.Ventas.DetalleVentaWindow(uiDetalle, _guardarDetalle)
            {
                Owner = System.Windows.Application.Current.MainWindow
            };
            if (win.ShowDialog() == true)
                await RecargarAsync();
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(ex.Message, "Error al abrir detalle",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        }
    }

    private static VentasApp.Desktop.ViewModels.DTOs.VentaDetalleDto MapVentaDetalle(
        VentasApp.Application.DTOs.Venta.VentaDetalleDto src)
    {
        return new VentasApp.Desktop.ViewModels.DTOs.VentaDetalleDto
        {
            Codigo = src.Codigo,
            Cliente = src.Cliente,
            IdCliente = src.IdCliente,
            Items = new ObservableCollection<VentaItemDto>(
                src.Items.Select(i => new VentaItemDto
                {
                    IdDetalle = i.IdDetalle,
                    ProductId = i.IdItemVendible,
                    Producto = i.Descripcion,
                    PrecioUnitario = i.PrecioUnitario,
                    Cantidad = i.Cantidad
                })),
            Pagos = new ObservableCollection<PagoDto>(
                src.Pagos.Select(p =>
                {
                    var metodo = p.Metodos.FirstOrDefault();
                    return new PagoDto
                    {
                        Id = p.Id,
                        Fecha = p.FechaPago,
                        Monto = p.Total,
                        MedioPago = metodo?.MedioPago ?? string.Empty,
                        MedioPagoId = metodo?.IdMedioPago ?? 0
                    };
                }))
        };
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
        await RecargarAsync();
    }

    public async Task RecargarAsync()
    {
        var list = await _clienteRepository.ListarClientes();
        var mapped = new List<ClienteCardDto>();

        using var scope = App.AppHost!.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VentasApp.Infrastructure.Persistencia.Contexto.DatabaseContext>();

        foreach (var c in list)
        {
            var ventas = db.Compras
                .Where(comp => comp.IdCliente == c.Id)
                .Join(db.Ventas,
                    comp => comp.IdVenta,
                    v => v.Id,
                    (comp, v) => new VentasApp.Desktop.ViewModels.DTOs.VentaResumenDto
                    {
                        Id = v.Id,
                        Fecha = v.FechaVenta,
                        Total = v.MontoTotal,
                        EstadoVenta = v.Estado.ToString()
                    })
                .OrderByDescending(v => v.Fecha)
                .Take(5)
                .ToList();

            var deudaTotal = (decimal)db.Compras
                .Where(comp => comp.IdCliente == c.Id)
                .Join(db.Ventas, comp => comp.IdVenta, v => v.Id, (comp, v) => (double)v.SaldoPendiente)
                .Sum();

            mapped.Add(new ClienteCardDto
            {
                Id = c.Id,
                Nombre = c.Nombre ?? string.Empty,
                Dni = c.DNI ?? string.Empty,
                Telefonos = string.Join(" / ", (c.Telefonos ?? new List<VentasApp.Domain.Modelo.Cliente.Telefono>()).Select(t => t.Numero)),
                DeudaTotal = deudaTotal,
                UltimasCompras = new ObservableCollection<VentasApp.Desktop.ViewModels.DTOs.VentaResumenDto>(ventas),
                VerDetalleVentaCommand = VerDetalleVentaCommand
            });
        }

        Clientes = new ObservableCollection<ClienteCardDto>(mapped);
    }
}