```text
GestionVentas.Domain
│
├── Base
│   ├── Entidad.cs              Clase base para todas las entidades, Define la propiedad de identidad (id) y centraliza el concepto de igualdad entre entidades 
│   └── ExcepcionDominio.cs     Excepcion personalizada utilizada para representar errores de negocio. Permite diferenciar violaciones de reglas del dominio de errores tecnicos o de infrarsctructura
│
├── Modelos
│   ├── Ventas
│   │   ├── Venta.cs            Representa una venta, administra el estado de la venta, los items vendidos, los pagos asociados, el total, el saldo pendiente y valida la transiciones de estado
│   │   └── DetalleVenta.cs     Representa cada item vendido dentro de una venta, contiene el producto vendido, la cantidad, el precio unitario y el calculo del subtotal
│   │
│   ├── Productos
│   │   ├── Producto.cs         Representa un producto comercializable, contiene la informacion basica del producto, y su relacion con el stock disponible
│   │   └── Stock.cs            Maneja la cantidad disponible de un producto,
│   │
│   ├── Pagos
│   │   ├── Pago.cs             Representa un pago realizado por un cliente, registra el monto, la fecha y el medio de pago utilizado, permitiendo pagos parciales y multiples pagos por venta
│   │   └── MedioPago.cs        Representa un medio de pago configurable (recargos, estado, etc)
│   │
│   └── Clientes
│   |   └── Cliente.cs          Representa un cliente el negocio, almacena los datos basicos de un cliente y permite su asosiacion opcional a una venta
│   |   └── Telefono.cs         Representa un numero de telefono valido, Encapsula la validacion y el formato del numero como un objeto inmutable
|   |
|   └── Categoria
|       └── Categoria.cs   
|
└── Enums
    ├── EstadoVenta.cs          Define los posible estados de una venta, Se utiliza para controlar el flujo y las transciciones validas del proceso venta
    └── TipoVenta.cs            Define los tipo de venta admitidos por el sistema, permite diferenciar comportamientos segun si la venta es al contado, a credito o por pedido
```