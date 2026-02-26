UI\VentasApp.Desktop\Views\Ventas\SeleccionarItemWindow.xaml.cs
using System;
using System.Linq;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using VentasApp.Domain.Modelo.Productos;
using VentasApp.Desktop.ViewModels.DTOs;
using VentasApp.Infrastructure.Persistencia.Contexto;

namespace VentasApp.Desktop.Views.Ventas;

public partial class SeleccionarItemWindow : Window
{
    public VentaItemDto? SelectedItem { get; private set; }

    public SeleccionarItemWindow()
    {
        InitializeComponent();
        Loaded += SeleccionarItemWindow_Loaded;
    }

    private async void SeleccionarItemWindow_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            using var scope = App.AppHost!.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            var items = await db.ItemVendible.Include(i => i.Producto).Where(i => i.Activado).ToListAsync();
            ItemsGrid.ItemsSource = items;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error cargando items: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Close();
        }
    }

    private void Agregar_Click(object sender, RoutedEventArgs e)
    {
        if (ItemsGrid.SelectedItem is not ItemVendible sel)
        {
            MessageBox.Show("Selecciona un item.", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!int.TryParse(CantidadTxt.Text, out var cantidad) || cantidad <= 0)
        {
            MessageBox.Show("Cantidad inválida.", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!decimal.TryParse(PrecioTxt.Text, out var precio) || precio < 0)
        {
            MessageBox.Show("Precio inválido.", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        SelectedItem = new VentaItemDto
        {
            Producto = sel.Nombre,
            PrecioUnitario = precio,
            Cantidad = cantidad
        };

        DialogResult = true;
        Close();
    }

    private void Cancelar_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}