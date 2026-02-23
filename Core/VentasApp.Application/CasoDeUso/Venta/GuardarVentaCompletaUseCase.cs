using VentasApp.Application.DTOs.Venta;
using VentasApp.Application.Interfaces.Repositorios;
using VentasApp.Domain.Base;

namespace VentasApp.Application.CasoDeUso.Venta;

public class GuardarVentaCompletaUseCase
{
    private readonly IVentaRepository _ventaRepo;
    private readonly IPagoRepository _pagoRepo;
    private readonly IMedioPagoRepository _medioRepo;
    private readonly IStockRepository _stockRepo;
    private readonly IItemVendibleRepository _itemRepo;
    private readonly IUnitOfWork _unitOfWork;

    public GuardarVentaCompletaUseCase(IVentaRepository ventaRepo, IPagoRepository pagoRepo, IMedioPagoRepository medioRepo, IStockRepository stockRepo, IItemVendibleRepository itemRepo, IUnitOfWork unit)
    {
        _ventaRepo = ventaRepo;
        _pagoRepo = pagoRepo;
        _medioRepo = medioRepo;
        _stockRepo = stockRepo;
        _itemRepo = itemRepo;
        _unitOfWork = unit;
    }

    public async Task EjecutarAsync(VentaDetalleDto dto)
    {
        System.Diagnostics.Debug.WriteLine($"[GUARDAR_VENTA_COMPLETA] Inicio - VentaId: {dto.Id}, Items: {dto.Items.Count}");
        
        var venta = await _ventaRepo.ObtenerPorId(dto.Id) ?? throw new Exception("Venta no encontrada");

        // Aplicar cambio de fecha si fue modificada
        if (dto.Fecha != venta.FechaVenta)
        {
            venta.ModificarFecha(dto.Fecha);
        }

        System.Diagnostics.Debug.WriteLine("[GUARDAR_VENTA_COMPLETA] Iniciando validación de stock");
        
        // PRE-VALIDAR todo el stock ANTES de hacer cualquier cambio
        // Esto evita que el DbContext quede en estado inconsistente si falla
        foreach (var item in dto.Items)
        {
            if (item.IdDetalle == 0)
            {
                // Nuevo item: validar stock completo
                var stock = await _stockRepo.ObtenerPorItemVendible(item.IdItemVendible);
                if (stock == null)
                    throw new ExcepcionDominio("No se encontró stock para el producto seleccionado");

                System.Diagnostics.Debug.WriteLine($"[VALIDACION] ItemVendible: {item.IdItemVendible}, Disponible: {stock.CantidadDisponible}, Solicitado: {item.Cantidad}");

                if (stock.CantidadDisponible < item.Cantidad)
                {
                    var itemVendible = await _itemRepo.ObtenerItem(item.IdItemVendible);
                    var nombre = itemVendible?.Nombre ?? "el producto";
                    if (!string.IsNullOrWhiteSpace(itemVendible?.Talle))
                        nombre = $"{nombre} - Talle {itemVendible.Talle}";
                    
                    System.Diagnostics.Debug.WriteLine($"[VALIDACION] Stock insuficiente detectado - Lanzando ExcepcionDominio");
                    throw new ExcepcionDominio($"Stock insuficiente para '{nombre}'.\n\nDisponible: {stock.CantidadDisponible} unidades\nSolicitado: {item.Cantidad} unidades\n\nPor favor, reduzca la cantidad o verifique el stock disponible.");
                }
            }
            else
            {
                // Item existente: validar solo si aumenta la cantidad
                var original = venta.Detalles.FirstOrDefault(d => d.Id == item.IdDetalle);
                if (original is not null && item.Cantidad > original.Cantidad)
                {
                    var diferencia = item.Cantidad - original.Cantidad;
                    var stock = await _stockRepo.ObtenerPorItemVendible(item.IdItemVendible);
                    if (stock == null)
                        throw new ExcepcionDominio("No se encontró stock para el producto");

                    if (stock.CantidadDisponible < diferencia)
                    {
                        var itemVendible = await _itemRepo.ObtenerItem(item.IdItemVendible);
                        var nombre = itemVendible?.Nombre ?? "el producto";
                        if (!string.IsNullOrWhiteSpace(itemVendible?.Talle))
                            nombre = $"{nombre} - Talle {itemVendible.Talle}";
                        throw new ExcepcionDominio($"Stock insuficiente para '{nombre}'.\n\nDisponible: {stock.CantidadDisponible} unidades\nAdicional requerido: {diferencia} unidades\n\nPor favor, ajuste la cantidad.");
                    }
                }
            }
        }

        // Todas las validaciones pasaron, proceder con los cambios
        var existentes = venta.Detalles.Select(d => d.Id).ToList();

        foreach (var item in dto.Items)
        {
            if (item.IdDetalle == 0)
            {
                var stock = await _stockRepo.ObtenerPorItemVendible(item.IdItemVendible);
                if (stock == null)
                {
                    throw new ExcepcionDominio($"No se encontró stock para el producto seleccionado");
                }
                
                System.Diagnostics.Debug.WriteLine($"[STOCK] Agregando detalle - ItemVendible: {item.IdItemVendible}, Stock disponible: {stock.CantidadDisponible}, Cantidad: {item.Cantidad}");
                
                venta.AgregarDetalle(item.IdItemVendible, item.Cantidad, item.PrecioUnitario);
                stock.Descontar(item.Cantidad);
                await _stockRepo.Actualizar(stock);
                
                System.Diagnostics.Debug.WriteLine($"[STOCK] Después de agregar - Stock disponible: {stock.CantidadDisponible}");
            }
            else
            {
                var original = venta.Detalles.FirstOrDefault(d => d.Id == item.IdDetalle);
                if (original is not null)
                {
                    if (original.Cantidad != item.Cantidad || original.PrecioUnitario != item.PrecioUnitario)
                    {
                        // Si la cantidad cambió, ajustar el stock
                        if (original.Cantidad != item.Cantidad)
                        {
                            var stock = await _stockRepo.ObtenerPorItemVendible(item.IdItemVendible);
                            if (stock == null)
                            {
                                throw new ExcepcionDominio($"No se encontró stock para el producto");
                            }
                            
                            var diferencia = item.Cantidad - original.Cantidad;
                            
                            System.Diagnostics.Debug.WriteLine($"[STOCK] Modificando cantidad - Diferencia: {diferencia}, Stock disponible: {stock.CantidadDisponible}");
                            
                            if (diferencia > 0)
                            {
                                try
                                {
                                    stock.Descontar(diferencia);
                                }
                                catch (ExcepcionDominio ex) when (ex.Message.Contains("Stock insuficiente"))
                                {
                                    var itemVendible = await _itemRepo.ObtenerItem(item.IdItemVendible);
                                    var nombre = itemVendible?.Nombre ?? "el producto";
                                    if (!string.IsNullOrWhiteSpace(itemVendible?.Talle))
                                        nombre = $"{nombre} - Talle {itemVendible.Talle}";
                                    throw new ExcepcionDominio($"Stock insuficiente para '{nombre}'.\n\nDisponible: {stock.CantidadDisponible} unidades\nAdicional requerido: {diferencia} unidades\n\nPor favor, ajuste la cantidad.");
                                }
                                await _stockRepo.Actualizar(stock);
                            }
                            else if (diferencia < 0)
                            {
                                stock.Aumentar(Math.Abs(diferencia));
                                await _stockRepo.Actualizar(stock);
                            }
                        }
                        
                        venta.ModificarDetalle(item.IdDetalle, item.Cantidad, item.PrecioUnitario);
                    }
                    
                    // Actualizar estado de entrega
                    if (item.Entregado != original.Entregado)
                    {
                        if (item.Entregado)
                        {
                            venta.MarcarItemComoEntregado(item.IdDetalle);
                        }
                        else
                        {
                            venta.DesmarcarItemEntregado(item.IdDetalle);
                        }
                    }
                    
                    existentes.Remove(item.IdDetalle);
                }
            }
        }

        foreach (var idToRemove in existentes)
        {
            // Devolver el stock antes de eliminar
            var detalleAEliminar = venta.Detalles.FirstOrDefault(d => d.Id == idToRemove);
            if (detalleAEliminar != null)
            {
                var stock = await _stockRepo.ObtenerPorItemVendible(detalleAEliminar.IdItemVendible);
                if (stock != null)
                {
                    stock.Aumentar(detalleAEliminar.Cantidad);
                    await _stockRepo.Actualizar(stock);
                }
            }
            
            venta.EliminarDetalle(idToRemove);
        }

        // Manejo pagos
        var pagosPersistidos = await _pagoRepo.ObtenerPorVenta(venta.Id);
        var idsPersistidos = pagosPersistidos.Select(p => p.Id).ToList();
        var idsEnDto = dto.Pagos.Select(p => p.Id).ToList();

        // Agregar nuevos pagos
        foreach (var pagoDto in dto.Pagos.Where(p => p.Id == 0))
        {
            var pago = new VentasApp.Domain.Modelo.Pago.Pago(venta.Id, false);
            
            if (pagoDto.FechaPago != default && pagoDto.FechaPago != pago.FechaPago)
            {
                pago.ModificarFecha(pagoDto.FechaPago);
            }
            
            foreach (var metodo in pagoDto.Metodos)
            {
                var medio = await _medioRepo.ObtenerPorId(metodo.IdMedioPago) ?? throw new Exception("Medio de pago invalido");
                pago.AgregarPago(medio.Id, metodo.Monto);
            }
            pago.ValidarPago();
            if (pagoDto.Verificado)
            {
                pago.MarcarComoVerificado();
            }
            
            venta.RegistrarPago(pago.Total);
            
            await _pagoRepo.Agregar(pago);
        }

        // Eliminar pagos removidos
        var idsAEliminar = idsPersistidos.Except(idsEnDto).ToList();
        foreach (var idEl in idsAEliminar)
        {
            // eliminar pago
            await _pagoRepo.Eliminar(idEl);
        }

        // Actualizar fechas de pagos existentes si fueron modificadas
        foreach (var pagoDto in dto.Pagos.Where(p => p.Id > 0))
        {
            var pagoExistente = pagosPersistidos.FirstOrDefault(p => p.Id == pagoDto.Id);
            if (pagoExistente != null && pagoExistente.FechaPago != pagoDto.FechaPago)
            {
                pagoExistente.ModificarFecha(pagoDto.FechaPago);
                await _pagoRepo.Actualizar(pagoExistente);
            }
        }

        // Calcular el total pagado teniendo en cuenta pagos persistidos (excluyendo los eliminados)
        // y los nuevos pagos incluidos en el DTO. Evita depender de una nueva consulta que
        // podría no incluir los pagos añadidos al contexto antes de SaveChanges.
        var totalPersistidos = pagosPersistidos.Where(p => !idsAEliminar.Contains(p.Id)).Sum(p => p.Total);
        var totalNuevos = dto.Pagos.Where(p => p.Id == 0).Sum(p => p.Total);
        var totalPagos = totalPersistidos + totalNuevos;
        venta.RecalcularMontosDesdePagos(totalPagos);

        // Vincular cliente si fue seleccionado
        if (dto.IdCliente > 0)
        {
            await _ventaRepo.VincularClienteVenta(venta.Id, dto.IdCliente);
        }

        // Actualizar venta y guardar todo en una sola transaccion
        await _ventaRepo.Actualizar(venta);
        await _unitOfWork.SaveChanges();
        
        // Asegurar que el monto pagado en la venta refleje exactamente los pagos
        // persistidos (por si hubiera discrepancias entre el contexto y la consulta)
        var pagosFinales = await _pagoRepo.ObtenerPorVenta(venta.Id);
        var totalPersistido = pagosFinales.Sum(p => p.Total);
        // Si difiere, recalculamos y persistimos de nuevo.
        if (totalPersistido != venta.MontoPagado)
        {
            venta.RecalcularMontosDesdePagos(totalPersistido);
            await _ventaRepo.Actualizar(venta);
            await _unitOfWork.SaveChanges();
        }
    }
}
