using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using VentasApp.Desktop.ViewModels.DTOs;
using VentasApp.Domain.Modelo.Cliente;

namespace VentasApp.Desktop.ViewModels.Cliente
{
    public partial class ClienteViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ClienteCardDto> _clientes;

        public ClienteViewModel()
        {
            // Cargar datos de prueba
            Clientes = new ObservableCollection<ClienteCardDto>
            {
                // CASO 1: Cliente ideal (Sin deuda, historial limpio)
                new ClienteCardDto
                {
                    Id = 1,
                    Nombre = "María González",
                    Dni = "30.123.456",
                    Telefonos = "2241-554433",
                    DeudaTotal = 0,
                    UltimasCompras = new ObservableCollection<VentaResumenDto>
                    {
                        new VentaResumenDto { IdVenta = 1025, Fecha = DateTime.Now.AddDays(-5), Total = 15000, Estado = "Pagada" },
                        new VentaResumenDto { IdVenta = 980, Fecha = DateTime.Now.AddMonths(-1), Total = 8500, Estado = "Pagada" }
                    }
                },

                // CASO 2: Cliente Deudor (Verás el borde rojo y la etiqueta)
                new ClienteCardDto
                {
                    Id = 2,
                    Nombre = "Carlos 'El Moroso' Pérez",
                    Dni = "25.987.654",
                    Telefonos = "11-1234-5678 / 11-9999-0000",
                    DeudaTotal = 45200.50m, // ¡Tiene deuda!
                    UltimasCompras = new ObservableCollection<VentaResumenDto>
                    {
                        new VentaResumenDto { IdVenta = 1050, Fecha = DateTime.Now.AddDays(-2), Total = 45200.50m, Estado = "Pendiente" },
                        new VentaResumenDto { IdVenta = 1001, Fecha = DateTime.Now.AddDays(-20), Total = 12000, Estado = "Pagada" }
                    }
                },

                // CASO 3: Cliente Nuevo (Sin historial)
                new ClienteCardDto
                {
                    Id = 3,
                    Nombre = "Escuela N° 1 (Cooperadora)",
                    Dni = "CUIT 30-11111111-9",
                    Telefonos = "2241-423311",
                    DeudaTotal = 0
                }
            };
        }
    }
}