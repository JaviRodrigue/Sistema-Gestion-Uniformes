using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using VentasApp.Desktop.ViewModels.DTOs;
using VentasApp.Desktop.ViewModels.Ventas;
using VentasApp.Application.CasoDeUso.DetalleVenta;
using System.Collections.ObjectModel;
using VentasApp.Application.DTOs.Productos;
using CommunityToolkit.Mvvm.Messaging;
using VentasApp.Desktop.Messages;
using VentasApp.Application.CasoDeUso.Productos;
using VentasApp.Application.CasoDeUso.Pago;

namespace VentasApp.Desktop.Views.Ventas;

    public partial class DetalleVentaWindow : Window
    {
        private readonly GuardarDetalleVentaUseCase _guardar;
        private readonly VentaDetalleDto _dto;

        public static readonly DependencyProperty ProductsProperty = DependencyProperty.Register(
            "Productos", typeof(System.Collections.Generic.List<VentasApp.Application.DTOs.Productos.ListadoProductoDto>), typeof(DetalleVentaWindow), new PropertyMetadata(null));

        public System.Collections.Generic.List<VentasApp.Application.DTOs.Productos.ListadoProductoDto> Productos
        {
            get => (System.Collections.Generic.List<VentasApp.Application.DTOs.Productos.ListadoProductoDto>)GetValue(ProductsProperty);
            set => SetValue(ProductsProperty, value);
        }

        public DetalleVentaWindow(VentaDetalleDto dto, GuardarDetalleVentaUseCase guardar)
        {
            InitializeComponent();
            _dto = dto;
            _guardar = guardar;
            
            // Create VM without Use Cases first, or pass them later?
            // DetalleVentaViewModel needs them in constructor.
            // Let's resolve them from a scope that lives as long as the window, or just resolve them inside CargarDatosAsync.
            // Actually, we can just pass the provider to VM or resolve them in CargarDatosAsync.
            // Let's change DetalleVentaViewModel to not require them in constructor, or we can create the scope and keep it alive.
            // Or we can just resolve them inside CargarDatosAsync in the Window and pass the data to VM.
            
            var vm = new DetalleVentaViewModel(dto);
            DataContext = vm;

            // Cargar datos asincrónicamente
            _ = CargarDatosAsync(vm);
        }

        private async System.Threading.Tasks.Task CargarDatosAsync(DetalleVentaViewModel vm)
        {
            var provider = App.AppHost!.Services;
            using var scope = provider.CreateScope();
            var obtenerProductos = scope.ServiceProvider.GetRequiredService<ObtenerProductosParaVentaUseCase>();
            var obtenerMediosPago = scope.ServiceProvider.GetRequiredService<ObtenerMediosPagoUseCase>();
            
            await vm.CargarDatosAsync(obtenerProductos, obtenerMediosPago);
            this.Productos = new System.Collections.Generic.List<VentasApp.Application.DTOs.Productos.ListadoProductoDto>(vm.Productos);
        }

        private static T? FindVisualChild<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) return null;
            for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = System.Windows.Media.VisualTreeHelper.GetChild(depObj, i);
                if (child is T t) return t;
                var result = FindVisualChild<T>(child);
                if (result != null) return result;
            }
            return null;
        }

        private void ComboBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (sender is null) return;
            var tb = e.OriginalSource as System.Windows.Controls.TextBox;
            if (tb == null) return;

            var combo = FindAncestor<System.Windows.Controls.ComboBox>(tb);
            if (combo == null) return;

            var vm = DataContext as DetalleVentaViewModel;
            if (vm == null) return;

            var text = tb.Text ?? string.Empty;
            var filtered = vm.Productos.Where(p => p.Nombre.Contains(text, System.StringComparison.OrdinalIgnoreCase)).ToList();
            combo.ItemsSource = filtered;
            combo.IsDropDownOpen = filtered.Any();
        }

        private static T? FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            while (current != null)
            {
                if (current is T) return (T)current;
                current = System.Windows.Media.VisualTreeHelper.GetParent(current);
            }
            return null;
        }

        private async void Guardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[GUARDAR_CLICK] Iniciando GuardarVentaAsync");
                await GuardarVentaAsync();
                System.Diagnostics.Debug.WriteLine("[GUARDAR_CLICK] GuardarVentaAsync completado exitosamente");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[GUARDAR_CLICK] Excepción capturada: {ex.GetType().Name} - {ex.Message}");
                
                
                if (ex is VentasApp.Domain.Base.ExcepcionDominio exDom)
                {
                    MessageBox.Show(exDom.Message, "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    MessageBox.Show($"Error inesperado: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task GuardarVentaAsync()
        {
            try
            {
                
                if (_dto.Estado == VentasApp.Domain.Enum.EstadoVenta.Cancelada)
                {
                    MessageBox.Show("No se puede modificar una venta cancelada.", "Venta cancelada", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                
                try
                {
                    this.MoveFocus(new System.Windows.Input.TraversalRequest(System.Windows.Input.FocusNavigationDirection.Next));
                }
                catch { }

                try
                {
                    ItemsDataGrid.CommitEdit(System.Windows.Controls.DataGridEditingUnit.Row, true);
                    ItemsDataGrid.CommitEdit();
                }
                catch { }
                try
                {
                    PagosDataGrid.CommitEdit(System.Windows.Controls.DataGridEditingUnit.Row, true);
                    PagosDataGrid.CommitEdit();
                }
                catch { }
                
                if (_dto.Items.Any(i => i.ProductId == 0))
                {
                    MessageBox.Show("Debe seleccionar un producto para cada línea antes de guardar.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var appDto = new VentasApp.Application.DTOs.Venta.VentaDetalleDto
                {
                    Id = _dto.Id,
                    Codigo = _dto.Codigo,
                    Cliente = _dto.Cliente,
                    Fecha = _dto.Fecha,
                    Items = _dto.Items.Select(i => new VentasApp.Application.DTOs.Venta.VentaItemDto
                    {
                        IdDetalle = i.IdDetalle,
                        Descripcion = i.Producto,
                        IdItemVendible = i.ProductId,
                        Cantidad = i.Cantidad,
                        PrecioUnitario = i.PrecioUnitario,
                        Entregado = i.Entregado
                    }).ToList(),
                    Pagos = _dto.Pagos.Select(p => new VentasApp.Application.DTOs.Pago.PagoDto
                    {
                        Id = p.Id,
                        IdVenta = _dto.Id,
                        FechaPago = p.Fecha,
                        Total = p.Monto,
                        Verificado = p.Verificado,
                        Metodos = new List<VentasApp.Application.DTOs.Pago.PagoMetodoDetalleDto>
                        {
                            new VentasApp.Application.DTOs.Pago.PagoMetodoDetalleDto { IdMedioPago = p.MedioPagoId, MedioPago = "", Monto = p.Monto }
                        }
                    }).ToList()
                };



                


                
                System.Diagnostics.Debug.WriteLine("[GUARDAR_VENTA_ASYNC] Llamando a GuardarVentaCompletaUseCase");
                using (var scope = App.AppHost!.Services.CreateScope())
                {
                    var guardarCompleto = scope.ServiceProvider.GetRequiredService<VentasApp.Application.CasoDeUso.Venta.GuardarVentaCompletaUseCase>();
                    await guardarCompleto.EjecutarAsync(appDto);
                    
                    // Vincular cliente a la venta via Compra si fue seleccionado
                    var vm2 = DataContext as DetalleVentaViewModel;
                    if (vm2?.IdCliente > 0)
                    {
                        var db = scope.ServiceProvider.GetRequiredService<VentasApp.Infrastructure.Persistencia.Contexto.DatabaseContext>();
                        var compraExistente = db.Compras.FirstOrDefault(c => c.IdVenta == _dto.Id);
                        
                        if (compraExistente != null)
                        {
                            if (compraExistente.IdCliente != vm2.IdCliente)
                            {
                                db.Compras.Remove(compraExistente);
                                db.Compras.Add(new VentasApp.Domain.Modelo.Venta.Compra(_dto.Id, vm2.IdCliente));
                                await db.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            db.Compras.Add(new VentasApp.Domain.Modelo.Venta.Compra(_dto.Id, vm2.IdCliente));
                            await db.SaveChangesAsync();
                        }
                    }
                }
                System.Diagnostics.Debug.WriteLine("[GUARDAR_VENTA_ASYNC] GuardarVentaCompletaUseCase completado exitosamente");

                

                
                try
                {
                    var ventaVm = App.AppHost!.Services.GetRequiredService<VentasApp.Desktop.ViewModels.Ventas.VentaViewModel>();
                    await ventaVm.RefreshAsync();
                }
                catch
                {
                    // ignore failures here; refresh is best-effort
                }

                WeakReferenceMessenger.Default.Send(new StockChangedMessage());

                DialogResult = true;
                Close();
            }
            catch (VentasApp.Domain.Base.ExcepcionDominio ex)
            {
                System.Diagnostics.Debug.WriteLine($"[GUARDAR_VENTA_ASYNC] ExcepcionDominio capturada: {ex.Message}");
                try { ItemsDataGrid.CancelEdit(); } catch { }
                try { PagosDataGrid.CancelEdit(); } catch { }
                MessageBox.Show(ex.Message, "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[GUARDAR_VENTA_ASYNC] Exception capturada: {ex.GetType().Name} - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[GUARDAR_VENTA_ASYNC] StackTrace: {ex.StackTrace}");
                try { ItemsDataGrid.CancelEdit(); } catch { }
                try { PagosDataGrid.CancelEdit(); } catch { }
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error al guardar", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnAgregarClienteClick(object sender, RoutedEventArgs e)
        {
            var selector = new VentasApp.Desktop.Views.Cliente.ListarClienteVentas { Owner = this };
            if (selector.ShowDialog() == true && selector.ClienteNombre != null)
            {
                var vm = DataContext as DetalleVentaViewModel;
                if (vm != null)
                {
                    vm.Cliente = selector.ClienteNombre;
                    vm.IdCliente = selector.IdCliente;
                }
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
