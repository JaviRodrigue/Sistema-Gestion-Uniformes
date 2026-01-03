```text
GestionVentas.Infrastructure
│
├── Persistencia
│ ├── Contexto
│ │   └── GestionVentasDbContext.cs  DbContext principal de Entity Framework Core (mapear entidades del dominio, exponer DbSets, configurar transacciones)
│ │
│ ├── Configuraciones               Clases de configuracion EF Core (IEntityTypeConfiguration<T>). Separan el modelo de dominio del esquema fisico de la base de datos
│ │   ├── ProductoConfig.cs         Configura tabla, claves, relaciones y restricciones del producto
│ │   ├── VentaConfig.cs            Configura la entidad venta y sus relaciones con DetalleVenta y Pago
│ │   ├── DetalleVentaConfig.cs     Configura la relacion entre venta y producto
│ │   ├── PagoConfig.cs             Configura pago parciales y relacion con medioPago
│ │   ├── MedioPagoConfig.cs        Configura medios de pago dinamicos
│ │   └── ClienteConfig.cs          Configur datos opcionales del cliente
│ │
│ ├── Repositorios                  Implementaciones concretas de las interfaces de Application
│ │   ├── ProductoRepository.cs     Implementa IProductoRepository usando EF Core.
│ │   ├── VentaRepository.cs        Implementa IVentaRepository (carga de agregados completos, manejo de transacciones)
│ │   ├── ClienteRepository.cs      Implementa IClienteRepository.
│ │   ├── PagoRepository.cs         Implementa IPagoRepository.
│ │   └── MedioPagoRepository.cs    Implementa IMedioPagoRepository.
│ │
│ └── Migraciones                   Carpeta generada por EF Core(Versionar la base de datos, aplicar cambios de esquema)
│
├── ServiciosExternos
│   ├── Impresion
│   │   └── ServicioImpresion.cs   Servicio para impresion de comprobantes no fiscales (Formatear tickets, Enviar a impresoras termica o comunes)
│   │
│   └── Facturacion
│       └── FacturacionFiscalStub.cs    
│
├── Archivos
│   └── BackupService.cs           Servicio para realizar copia de seguridad (exportar base de datos, configurar frecuencias, soporte local o externo)
│
└── DependencyInjection
    └── InfrastructureServiceRegistration.cs   Clase centralizada para registrar dependencias (Registrar DbContext, Registrar repositorios, Registrar servicios externos)

```