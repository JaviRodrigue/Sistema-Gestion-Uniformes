using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VentasApp.Desktop.ViewModels;
using VentasApp.Desktop.ViewModels.DTOs;
using VentasApp.Desktop.Messages;

namespace VentasApp.Desktop.ViewModels.Productos
{
    public partial class ProductoViewModel : ObservableObject, IBuscable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly VentasApp.Application.Interfaces.Repositorios.IProductoRepository _productoRepository;
        private readonly VentasApp.Application.Interfaces.Repositorios.IStockRepository _stockRepository;
        private readonly VentasApp.Application.CasoDeUso.Productos.CrearProductoUseCase _crearProductoUseCase;
        private readonly VentasApp.Application.CasoDeUso.Productos.ActualizarProductoUseCase _actualizarProductoUseCase;
        private readonly VentasApp.Application.CasoDeUso.ItemVendibles.CrearItemVendibleUseCase _crearItemVendibleUseCase;
        private readonly VentasApp.Application.CasoDeUso.ItemVendibles.ActualizarItemVendibleUseCase _actualizarItemVendibleUseCase;
        private readonly VentasApp.Application.CasoDeUso.Stocks.CrearStockUseCase _crearStockUseCase;
        private readonly VentasApp.Application.CasoDeUso.Stocks.AumentarStockUseCase _aumentarStockUseCase;
        private readonly VentasApp.Application.CasoDeUso.Stocks.DescontarStockUseCase _descontarStockUseCase;
        private readonly VentasApp.Application.CasoDeUso.Stocks.ActualizarStockMinimoUseCase _actualizarStockMinimoUseCase;
        private readonly VentasApp.Desktop.Services.AppSettingsService _appSettingsService;

        private List<ProductoCardDto> _todosLosProductos = new();

        private readonly VentasApp.Application.Interfaces.Repositorios.IItemVendibleRepository _itemVendibleRepository;

        [ObservableProperty]
        private ObservableCollection<ProductoCardDto> _productos;

        [ObservableProperty]
        private string _filtroCategoria = "Todas";

        [ObservableProperty]
        private string _filtroStock = "Todos";

        public List<string> FiltrosCategoria { get; } = new() { "Todas", "Uniforme", "Libreria" };
        public List<string> FiltrosStock { get; } = new() { "Todos", "Bajo Stock", "Con Stock" };

        partial void OnFiltroCategoriaChanged(string value) => AplicarFiltros();
        partial void OnFiltroStockChanged(string value) => AplicarFiltros();

        private List<ProductoCardDto> _productosBusqueda = new();

        public ProductoViewModel(
            IServiceProvider serviceProvider,
            VentasApp.Application.Interfaces.Repositorios.IProductoRepository productoRepository,
            VentasApp.Application.Interfaces.Repositorios.IStockRepository stockRepository,
            VentasApp.Application.Interfaces.Repositorios.IItemVendibleRepository itemVendibleRepository,
            VentasApp.Application.CasoDeUso.Productos.CrearProductoUseCase crearProductoUseCase,
            VentasApp.Application.CasoDeUso.Productos.ActualizarProductoUseCase actualizarProductoUseCase,
            VentasApp.Application.CasoDeUso.ItemVendibles.CrearItemVendibleUseCase crearItemVendibleUseCase,
            VentasApp.Application.CasoDeUso.ItemVendibles.ActualizarItemVendibleUseCase actualizarItemVendibleUseCase,
            VentasApp.Application.CasoDeUso.Stocks.CrearStockUseCase crearStockUseCase,
            VentasApp.Application.CasoDeUso.Stocks.AumentarStockUseCase aumentarStockUseCase,
            VentasApp.Application.CasoDeUso.Stocks.DescontarStockUseCase descontarStockUseCase,
            VentasApp.Application.CasoDeUso.Stocks.ActualizarStockMinimoUseCase actualizarStockMinimoUseCase,
            VentasApp.Desktop.Services.AppSettingsService appSettingsService)
        {
            _serviceProvider = serviceProvider;
            _productoRepository = productoRepository;
            _stockRepository = stockRepository;
            _itemVendibleRepository = itemVendibleRepository;
            _crearProductoUseCase = crearProductoUseCase;
            _actualizarProductoUseCase = actualizarProductoUseCase;
            _crearItemVendibleUseCase = crearItemVendibleUseCase;
            _actualizarItemVendibleUseCase = actualizarItemVendibleUseCase;
            _crearStockUseCase = crearStockUseCase;
            _aumentarStockUseCase = aumentarStockUseCase;
            _descontarStockUseCase = descontarStockUseCase;
            _actualizarStockMinimoUseCase = actualizarStockMinimoUseCase;
            _appSettingsService = appSettingsService;
            Productos = new ObservableCollection<ProductoCardDto>();
            
            WeakReferenceMessenger.Default.Register<StockChangedMessage>(this, async (r, m) =>
            {
                await System.Windows.Application.Current.Dispatcher.InvokeAsync(async () =>
                {
                    await CargarProductosAsync();
                });
            });
            
            _ = CargarProductosAsync();
        }

        // ================= IBuscable =================

        public async Task BuscarAsync(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) { await RestablecerAsync(); return; }

            var textoLower = texto.ToLowerInvariant();
            var esNumero = texto.All(char.IsDigit);
            var resultados = new HashSet<ProductoCardDto>();

            if (esNumero)
            {
                // Buscar por ID
                if (int.TryParse(texto, out var id))
                {
                    var porId = _todosLosProductos.FirstOrDefault(p => p.Id == id);
                    if (porId != null) resultados.Add(porId);
                }
                
                // Buscar por código de barras
                var item = await _itemVendibleRepository.ObtenerItemPorCodigoBarra(texto);
                if (item != null)
                {
                    var porCodigo = _todosLosProductos.FirstOrDefault(p => p.Id == item.IdProducto);
                    if (porCodigo != null) resultados.Add(porCodigo);
                }
            }
            
            // Buscar por nombre, categoría o talle
            var porTexto = _todosLosProductos.Where(p => 
                p.Nombre.Contains(texto, System.StringComparison.OrdinalIgnoreCase) ||
                p.Categoria.Contains(texto, System.StringComparison.OrdinalIgnoreCase) ||
                (p.Talle != null && p.Talle.Contains(texto, System.StringComparison.OrdinalIgnoreCase)) ||
                p.CodigoBarraReferencia.Contains(texto, System.StringComparison.OrdinalIgnoreCase)
            );
            
            foreach (var p in porTexto)
            {
                resultados.Add(p);
            }

            _productosBusqueda = resultados.ToList();
            AplicarFiltros();
        }


        public Task RestablecerAsync()
        {
            _productosBusqueda = _todosLosProductos.ToList();
            AplicarFiltros();
            return Task.CompletedTask;
        }

        public async Task RefreshAsync()
        {
            await CargarProductosAsync();
        }


        [RelayCommand]
        private async void AgregarProducto()
        {
            var win = new Views.Productos.AgregarProductoWindow
            {
                Owner = System.Windows.Application.Current.MainWindow
            };
            var ok = win.ShowDialog();
            if (ok == true)
            {
                var nombre = (win.FindName("TxtNombre") as System.Windows.Controls.TextBox)?.Text ?? string.Empty;
                var categoriaItem = (win.FindName("CmbCategoria") as System.Windows.Controls.ComboBox)?.SelectedItem as System.Windows.Controls.ComboBoxItem;
                var costoText = (win.FindName("TxtCosto") as System.Windows.Controls.TextBox)?.Text ?? "0";
                var precioVentaText = (win.FindName("TxtPrecioVenta") as System.Windows.Controls.TextBox)?.Text ?? "0";

                decimal.TryParse(costoText, out var costo);
                decimal.TryParse(precioVentaText, out var precioVenta);

                // IdCategoria: determinar según selección simple (Uniforme=1, Librería=2) como mínimo viable
                var idCategoria = categoriaItem?.Content?.ToString() switch
                {
                    "Uniforme" => 1,
                    "Librería" => 2,
                    _ => 1
                };

                var dto = new VentasApp.Application.DTOs.Productos.CrearProductoDto
                {
                    IdCategoria = idCategoria,
                    Nombre = nombre,
                    Costo = costo,
                    PrecioVenta = precioVenta
                };

                var nuevoId = await _crearProductoUseCase.EjecutarAsync(dto);

                // Crear un ItemVendible base para el producto (requerido para asignar stock)
                var codigoBarra = (win.FindName("TxtCodigoBarra") as System.Windows.Controls.TextBox)?.Text?.Trim();
                if (string.IsNullOrWhiteSpace(codigoBarra))
                    codigoBarra = $"PROD-{nuevoId}";

                var itemDto = new VentasApp.Application.DTOs.ItemVendible.CrearItemVendibleDto
                {
                    IdProducto = nuevoId,
                    nombre = nombre,
                    CodigoBarra = codigoBarra,
                    Talle = win.TalleSeleccionado
                };
                // Crear stock si el usuario ingresó valores
                var cantidadInicialText = (win.FindName("TxtCantidadInicial") as System.Windows.Controls.TextBox)?.Text ?? string.Empty;
                var stockMinimoText = (win.FindName("TxtStockMinimo") as System.Windows.Controls.TextBox)?.Text ?? string.Empty;
                int.TryParse(cantidadInicialText, out var cantidadInicial);
                
                if (!int.TryParse(stockMinimoText, out var stockMinimo))
                {
                    stockMinimo = idCategoria == 1 ? _appSettingsService.StockMinimoUniforme : _appSettingsService.StockMinimoLibreria;
                }

                try
                {
                    var nuevoItemId = await _crearItemVendibleUseCase.EjecutarAsync(itemDto);

                    var stockDto = new VentasApp.Application.DTOs.Stocks.CrearStockDto
                    {
                        IdItemVendible = nuevoItemId,
                        CantidadInicial = cantidadInicial,
                        StockMinimo = stockMinimo
                    };
                    await _crearStockUseCase.EjecutarAsync(stockDto);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message, "Producto duplicado",
                        System.Windows.MessageBoxButton.OK,
                        System.Windows.MessageBoxImage.Warning);
                }
                finally
                {
                    await CargarProductosAsync();
                }
            }
        }

        [RelayCommand]
        private async void EditarProducto(ProductoCardDto? producto)
        {
            if (producto is null) return;
            var win = new Views.Productos.EditarProductoWindow
            {
                Owner = System.Windows.Application.Current.MainWindow
            };
            // Prefill fields
            (win.FindName("TxtNombre") as System.Windows.Controls.TextBox)!.Text = producto.Nombre;
            (win.FindName("TxtCosto") as System.Windows.Controls.TextBox)!.Text = producto.Precio.ToString();
            (win.FindName("TxtPrecioVenta") as System.Windows.Controls.TextBox)!.Text = producto.Precio.ToString();
            (win.FindName("TxtCantidadInicial") as System.Windows.Controls.TextBox)!.Text = producto.StockTotal.ToString();
            win.SetTalle(producto.Talle);

            var ok = win.ShowDialog();
            if (ok == true)
            {
                var nombre = (win.FindName("TxtNombre") as System.Windows.Controls.TextBox)?.Text ?? producto.Nombre;
                var costoText = (win.FindName("TxtCosto") as System.Windows.Controls.TextBox)?.Text ?? producto.Precio.ToString();
                var precioVentaText = (win.FindName("TxtPrecioVenta") as System.Windows.Controls.TextBox)?.Text ?? producto.Precio.ToString();

                decimal.TryParse(costoText, out var costo);
                decimal.TryParse(precioVentaText, out var precioVenta);

                var dto = new VentasApp.Application.DTOs.Productos.ActualizarProductoDto
                {
                    Nombre = nombre,
                    Costo = costo,
                    PrecioVenta = precioVenta
                };

                try
                {
                    // Update talle on the item vendible first to check for duplicates
                    var productoParaTalle = await _productoRepository.ObtenerProducto(producto.Id);
                    var itemParaTalle = productoParaTalle?.ItemsVendibles?.FirstOrDefault();
                    if (itemParaTalle is not null)
                    {
                        await _actualizarItemVendibleUseCase.EjecutarAsync(itemParaTalle.Id,
                            new VentasApp.Application.DTOs.ItemVendible.ActualizarItemVendibleDto
                            {
                                Nombre = nombre,
                                CodigoBarra = itemParaTalle.CodigoBarra,
                                Talle = win.TalleSeleccionado
                            });
                    }

                    await _actualizarProductoUseCase.EjecutarAsync(producto.Id, dto);

                    // Ajustar stock si el usuario ingresó un valor de cantidad
                var cantidadText = (win.FindName("TxtCantidadInicial") as System.Windows.Controls.TextBox)?.Text ?? string.Empty;
                var stockMinimoText = (win.FindName("TxtStockMinimo") as System.Windows.Controls.TextBox)?.Text ?? string.Empty;
                int.TryParse(cantidadText, out var nuevaCantidad);
                int.TryParse(stockMinimoText, out var nuevoMinimo);
                var cantidadIngresada = !string.IsNullOrWhiteSpace(cantidadText) && int.TryParse(cantidadText, out _);
                var minimoIngresado = !string.IsNullOrWhiteSpace(stockMinimoText) && int.TryParse(stockMinimoText, out _);

                if (cantidadIngresada || minimoIngresado)
                {
                    var productoDb = await _productoRepository.ObtenerProducto(producto.Id);
                    var item = productoDb?.ItemsVendibles?.FirstOrDefault();

                    // Si no existe ItemVendible, crear uno base
                    if (item is null)
                    {
                        var codigoBarra = $"PROD-{producto.Id}";
                        var existe = await _crearItemVendibleUseCase._repository.ObtenerItemPorCodigoBarra(codigoBarra);
                        if (existe is null)
                        {
                            var nuevoItemId = await _crearItemVendibleUseCase.EjecutarAsync(new VentasApp.Application.DTOs.ItemVendible.CrearItemVendibleDto
                            {
                                IdProducto = producto.Id,
                                nombre = producto.Nombre,
                                CodigoBarra = codigoBarra,
                                Talle = null
                            });
                            item = await _crearItemVendibleUseCase._repository.ObtenerItem(nuevoItemId);
                        }
                        else
                        {
                            item = existe;
                        }
                    }

                    if (item is not null)
                    {
                        var stock = await _stockRepository.ObtenerPorItemVendible(item.Id);
                        if (stock is null)
                        {
                            // No tiene stock todavía, crear con los valores indicados
                            await _crearStockUseCase.EjecutarAsync(new VentasApp.Application.DTOs.Stocks.CrearStockDto
                            {
                                IdItemVendible = item.Id,
                                CantidadInicial = cantidadIngresada ? nuevaCantidad : 0,
                                StockMinimo = minimoIngresado ? nuevoMinimo : 0
                            });
                        }
                        else
                        {
                            // Ajustar cantidad si se ingresó
                            if (cantidadIngresada)
                            {
                                var diferencia = nuevaCantidad - stock.CantidadDisponible;
                                if (diferencia > 0)
                                    await _aumentarStockUseCase.EjecutarAsync(item.Id, new VentasApp.Application.DTOs.Stocks.ActualizarStockDto { Cantidad = diferencia });
                                else if (diferencia < 0)
                                    await _descontarStockUseCase.EjecutarAsync(item.Id, -diferencia);
                            }
                            // Actualizar stock mínimo si se ingresó
                            if (minimoIngresado)
                                await _actualizarStockMinimoUseCase.EjecutarAsync(item.Id, nuevoMinimo);
                        }
                    }
                }
                }
                catch (System.Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message, "Error al editar producto",
                        System.Windows.MessageBoxButton.OK,
                        System.Windows.MessageBoxImage.Warning);
                }

                await CargarProductosAsync();
            }
        }

        [RelayCommand]
        private async Task EliminarProducto(ProductoCardDto? producto)
        {
            if (producto is null) return;
            
            var result = System.Windows.MessageBox.Show($"¿Está seguro que desea eliminar el producto {producto.Nombre}?", "Confirmar eliminación", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);
            if (result != System.Windows.MessageBoxResult.Yes) return;

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var eliminarUseCase = scope.ServiceProvider.GetRequiredService<VentasApp.Application.CasoDeUso.Productos.EliminarProductoUseCase>();
                await eliminarUseCase.EjecutarAsync(producto.Id);
                
                await CargarProductosAsync();
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error al eliminar producto",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
        }

        private async Task CargarProductosAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var productoRepository = scope.ServiceProvider.GetRequiredService<VentasApp.Application.Interfaces.Repositorios.IProductoRepository>();
            var stockRepository = scope.ServiceProvider.GetRequiredService<VentasApp.Application.Interfaces.Repositorios.IStockRepository>();
            
            var lista = await productoRepository.ListarProductos();
            var result = new List<ProductoCardDto>();
            foreach (var p in lista)
            {
                var item = p.ItemsVendibles?.FirstOrDefault();
                int stockTotal = 0;
                int stockMinimo = 0;
                if (item is not null)
                {
                    var stock = await stockRepository.ObtenerPorItemVendible(item.Id);
                    stockTotal = stock?.CantidadDisponible ?? 0;
                    stockMinimo = stock?.StockMinimo ?? 0;
                }

                result.Add(new ProductoCardDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre ?? string.Empty,
                    Categoria = p.IdCategoria switch
                    {
                        1 => "Uniforme",
                        2 => "Librería",
                        _ => "Otro"
                    },
                    Precio = p.PrecioVenta,
                    StockTotal = stockTotal,
                    StockMinimo = stockMinimo,
                    CodigoBarraReferencia = item?.CodigoBarra ?? string.Empty,
                    Talle = item?.Talle
                });
            }

            _todosLosProductos = result;
            _productosBusqueda = _todosLosProductos.ToList();
            AplicarFiltros();
        }

        private void AplicarFiltros()
        {
            var filtrados = _productosBusqueda.AsEnumerable();
            
            if (FiltroCategoria != "Todas")
            {
                // Handle the accent difference in the data vs filter
                var catFilter = FiltroCategoria == "Libreria" ? "Librería" : FiltroCategoria;
                filtrados = filtrados.Where(p => p.Categoria == catFilter);
            }
                
            if (FiltroStock == "Bajo Stock")
                filtrados = filtrados.Where(p => p.StockTotal <= p.StockMinimo);
            else if (FiltroStock == "Con Stock")
                filtrados = filtrados.Where(p => p.StockTotal > p.StockMinimo);

            Productos = new ObservableCollection<ProductoCardDto>(filtrados);
        }
    }
}