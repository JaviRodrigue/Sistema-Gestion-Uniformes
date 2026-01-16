```text
GestionVentas.Application
│
├── Interfaces
│  └── Repositorios
│     ├── IProductoRepository.cs        Define las operaciones necesarias para obtener y persistir productos (Buscar producto, Guardar cambios de producto)
│     ├── IVentaRepository.cs           Define las operaciones necesarios para manejar ventas (Crear ventas, obtener ventas por ID, guardar cambios de una venta)
│     ├── IClienteRepository.cs         Define las operaciones necesaras para manejar clientes (crear cliente, obtener cliente existentes)
│     ├── IPagoRepository.cs            Define las operaciones necesarias para persistir pagos (guardar pagos, consultar pagos asociados a ventas)
│     └── IMedioPagoRepository.cs       Define las operaciones necesarias para administrar medio configurable (Crear y actualizar medio de pagos, obtener medios de pago activos)
│
├── CasosDeUso
│ ├── Ventas
│ │  ├── CrearVentaUseCase.cs           Responsable de crear una nueva venta (crear una instancia de venta, Asociar cliente si corresponde, persisitir la venta inicial)
│ │  ├── AgregarDetalleVentaUseCase.cs  Responsable de agregar productos a una venta (validar que la venta este en estado valido, agregar un detalle a la venta, actualizar stock)
│ │  ├── ConfirmarVentaUseCase.cs       Responsable de confirmar una venta (Validar que la venta tenga items, cambiar el estado de la venta, Guardar cambios)
│ │  └── RegistrarPagoVentaUseCase.cs   Responsable de registrar pagos sobre una venta (Validar medio de pago, registrar pagos parciales o totales, actualizar estado de venta)
│ │
│ ├── Productos
│ │  ├── CrearProductoUseCase.cs        Responsable de crear un nuevo producto (validar datos del producto, crear producto y stock inicial, persistir el producto)
│ │  └── ActualizarStockUseCase.cs      Reponsable de modificar el stock del producto (incrementar o reducir el stock, validar cantidades)
│ │
│ ├── Clientes
│ │  └── CrearClienteUseCase.cs         Responsable de crear un nuevo cliente (validar datos basicos, persistir el cliente)
│ │
│ └── Pagos
│    └── CrearMedioPagoUseCase.cs       Responsable de crear o modificar un medio de pago (Definir recargo, activar o desactivar medio de pago)
│
├── DTOs
│   ├── VentaDto.cs                     Representa los datos de una venta para presentacion o entrada de datos
│   ├── DetalleVentaDto.cs              Representa los datos de un item de venta
│   ├── ProductoDto.cs                  Representa los datos de un producto
│   ├── ClienteDto.cs                   Representa los datos de un cliente
│   └── PagoDto.cs                      Representa los datos de un pago
│
└── Excepciones
    └── ExcepcionAplicacion.cs          Excepcion utilzada para errores propios de la logica de aplicacion
                                        Permite:envolver errores de dominio, manejar errores de flujo, comunicar errores a UI
```