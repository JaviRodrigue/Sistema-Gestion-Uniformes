# VentasApp.Desktop - Sistema de Gestión de Uniformes y Librería

## 📋 Descripción

Aplicación de escritorio WPF para gestión de ventas de uniformes escolares y artículos de librería. Implementa arquitectura limpia con patrón MVVM y Material Design.

## 🏗️ Arquitectura

### Estructura de Capas

```
Sistema-Gestion-Uniformes/
├── Core/
│   ├── VentasApp.Domain/          # Entidades y lógica de negocio
│   └── VentasApp.Application/     # Casos de uso
├── Infrastructure/
│   └── VentasApp.Infrastructure/  # Persistencia y servicios externos
└── UI/
    └── VentasApp.Desktop/         # Interfaz WPF (.NET 8)
```

### Proyecto Desktop

```
VentasApp.Desktop/
├── Views/
│   ├── Main/
│   │   └── MainWindow.xaml        # Ventana principal
│   ├── Cliente/
│   │   ├── ClienteView.xaml       # Lista de clientes
│   │   └── AgregarClienteWindow.xaml  # Modal alta cliente
│   └── Productos/
│       ├── ProductoView.xaml      # Lista de productos
│       └── AgregarProductoWindow.xaml # Modal alta producto
├── ViewModels/
│   ├── Cliente/
│   │   └── ClienteViewModel.cs    # Lógica de vista clientes
│   ├── Productos/
│   │   └── ProductoViewModel.cs   # Lógica de vista productos
│   └── DTOs/
│       ├── ClienteCardDto.cs      # DTO para tarjetas de cliente
│       └── ProductoCardDto.cs     # DTO para tarjetas de producto
├── Resources/
│   ├── Styles/
│   │   └── Colors.xaml            # Paleta de colores
│   └── Templates/
│       ├── ClienteCardTemplate.xaml   # Template tarjeta cliente
│       └── ProductoCardTemplate.xaml  # Template tarjeta producto
├── Converters/
│   └── DeudaColorConverter.cs     # Converter para color según deuda
└── App.xaml                        # Configuración global y recursos
```

## 🎨 Diseño y UI

### Paleta de Colores (Colors.xaml)

| Recurso          | Color                                                                      | Uso                              |
|------------------|----------------------------------------------------------------------------|----------------------------------|
| PrimaryPastel    | #F3E5F5   <span style="display:inline-block;width:16px;height:16px;background:#F3E5F5;border:1px solid #ccc;"></span> | Fondo iconos uniformes           |
| SecondaryPastel  | #E1F5FE   <span style="display:inline-block;width:16px;height:16px;background:#E1F5FE;border:1px solid #ccc;"></span> | Fondo general, botón editar      |
| AccentBlue       | #64B5F6   <span style="display:inline-block;width:16px;height:16px;background:#64B5F6;border:1px solid #ccc;"></span> | Botones principales, precios     |
| AccentRed        | #E57373   <span style="display:inline-block;width:16px;height:16px;background:#E57373;border:1px solid #ccc;"></span> | Botón eliminar, alertas deudores |
| TextDark         | #424242   <span style="display:inline-block;width:16px;height:16px;background:#424242;border:1px solid #ccc;"></span> | Texto principal                  |
| TextLight        | #757575   <span style="display:inline-block;width:16px;height:16px;background:#757575;border:1px solid #ccc;"></span> | Texto secundario                 |

### Componentes Principales

#### ProductoCardTemplate
Tarjeta de producto con:
- Icono según categoría (remera para Uniformes, libro para Librería)
- Badge de categoría
- Nombre y código de barras
- Precio destacado
- Badge de stock (rojo si stock = 0)
- Botones de acción: Editar (pastel) y Eliminar (rojo)

#### ClienteCardTemplate
Tarjeta de cliente expandible con:
- Avatar circular
- Nombre, DNI y teléfonos
- Badge "DEUDOR" si tiene deuda
- Monto de deuda destacado
- Historial de compras recientes (DataGrid)
- Botones de acción: Editar y Eliminar

## 📊 Entidades del Dominio

### Cliente
```csharp
public class Cliente : Entidad
{
    public string? DNI { get; private set; }
    public string? Nombre { get; private set; }
    public DateTime FechaAlta { get; private set; }
    public List<Telefono> Telefonos { get; private set; }
    public bool Activado { get; private set; }
}
```

**Campos obligatorios:** Nombre, DNI  
**Campos opcionales:** Teléfonos (lista), Email, Dirección

### Producto
```csharp
public class Producto : Entidad
{
    public int IdCategoria { get; private set; }
    public string Nombre { get; private set; }
    public decimal Costo { get; private set; }
    public decimal PrecioVenta { get; private set; }
    public decimal Ganancia => PrecioVenta - Costo;
    public bool Activo;
    private readonly List<ItemVendible> _itemVendibles;
}
```

**Campos obligatorios:** Nombre, IdCategoria, Costo, PrecioVenta  
**Validaciones:**
- `Costo > 0`
- `PrecioVenta > 0`
- `PrecioVenta >= Costo`

### ItemVendible
Representa variantes de producto (ej. talles, colores) con código de barras único.

## 🔧 Funcionalidades Implementadas

### Vista de Productos
- ✅ Listado de productos en tarjetas
- ✅ Botón "NUEVO PRODUCTO" (modal)
- ✅ Editar producto (botón en tarjeta)
- ✅ Eliminar producto (botón en tarjeta)
- ✅ Filtrado por categoría (visual)
- ✅ Badge de stock agotado

### Vista de Clientes
- ✅ Listado de clientes en tarjetas expandibles
- ✅ Botón "NUEVO CLIENTE" (modal)
- ✅ Editar cliente (botón en tarjeta)
- ✅ Eliminar cliente (botón en tarjeta)
- ✅ Identificación visual de deudores
- ✅ Historial de compras por cliente

### Formularios Modales

#### AgregarProductoWindow
**Campos:**
- Nombre (obligatorio)
- Categoría (obligatorio): Combo Uniforme/Librería
- Costo (obligatorio): Numérico, > 0
- Precio de Venta (obligatorio): Numérico, >= Costo
- Código de barras (opcional): Soporta lectora de códigos

**Validaciones:**
- Campos obligatorios no vacíos
- Valores numéricos válidos
- Reglas de dominio (precio >= costo)

#### AgregarClienteWindow
**Campos:**
- Nombre (obligatorio)
- DNI / CUIT (obligatorio)
- Teléfonos (opcional)
- Email (opcional)
- Dirección (opcional)

**Validaciones:**
- Nombre y DNI no vacíos

## 🛠️ Tecnologías y Paquetes

### .NET 8.0 (WPF)
- **CommunityToolkit.Mvvm** v8.4.0 - MVVM source generators
- **MaterialDesignThemes** v5.3.0 - Controles Material Design
- **MaterialDesignColors** v5.3.0 - Paleta de colores MD
- **Microsoft.Extensions.Hosting** v10.0.2 - DI y configuración
- **Microsoft.Extensions.Configuration** v10.0.2

## 📝 Convenciones de Código

### XAML

1. **Namespaces alineados:** 
   - `x:Class` y `namespace` deben coincidir entre `.xaml` y `.xaml.cs`
   - Vistas en `Views.*`, ViewModels en `ViewModels.*`

2. **MaterialDesign:**
   - Incluir `xmlns:materialDesign="http://materialdesigninxaml.net/winfx/xaml/themes"` al usar `PackIcon`
   - Usar recursos existentes (ej. `AccentBlue` en lugar de `PrimaryColor`)

3. **Layouts:**
   - **NO usar** `StackPanel.Spacing` (no existe en WPF estándar)
   - Usar `Margin` en elementos hijos para separación

4. **Botones:**
   - Envolver contenido múltiple (icono + texto) en `StackPanel` interno:
     ```xaml
     <Button>
         <StackPanel Orientation="Horizontal">
             <materialDesign:PackIcon Kind="Plus"/>
             <TextBlock Text="Agregar"/>
         </StackPanel>
     </Button>
     ```

### C# (ViewModels)

1. **CommunityToolkit.Mvvm:**
   - Usar `[ObservableProperty]` para propiedades observables
   - Usar `[RelayCommand]` para comandos
   - **NO duplicar** comandos con mismo nombre

2. **ObservableCollection:**
   - Asignar a la **propiedad generada** (ej. `Productos`), no al campo privado (`_productos`)
   - Ejemplo correcto:
     ```csharp
     [ObservableProperty]
     private ObservableCollection<ProductoCardDto> _productos;
     
     public ProductoViewModel()
     {
         Productos = new ObservableCollection<ProductoCardDto> { ... };
     }
     ```

3. **Comandos en modales:**
   - Usar `DialogResult = true` al guardar con éxito
   - `IsCancel="True"` en botón cancelar

## 🚀 Uso de la Aplicación

### Iniciar la Aplicación

```bash
cd UI/VentasApp.Desktop
dotnet run
```

### Agregar un Producto

1. Click en botón "NUEVO PRODUCTO"
2. Completar formulario:
   - Nombre del producto
   - Seleccionar categoría
   - Ingresar costo
   - Ingresar precio de venta (debe ser >= costo)
   - (Opcional) Escanear código de barras con pistola lectora
3. Click en "Guardar"

### Agregar un Cliente

1. Click en botón "NUEVO CLIENTE"
2. Completar formulario:
   - Nombre completo
   - DNI o CUIT
   - (Opcional) Teléfonos, email, dirección
3. Click en "Guardar"

### Editar/Eliminar

- Click en botón "Editar" (azul) en tarjeta → abre modal de edición
- Click en botón "Eliminar" (rojo) en tarjeta → confirma y elimina

## 🔄 Próximas Implementaciones

- [ ] Conectar ViewModels a casos de uso reales (UseCase)
- [ ] Implementar Repository pattern para persistencia
- [ ] Agregar validaciones de negocio desde dominio
- [ ] Implementar búsqueda y filtros avanzados
- [ ] Gestión de stock por ItemVendible
- [ ] Módulo de Ventas y DetalleVenta
- [ ] Reportes y estadísticas
- [ ] Gestión de categorías desde UI

## 📚 Referencias

- [Material Design in XAML Toolkit](http://materialdesigninxaml.net/)
- [CommunityToolkit.Mvvm Docs](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/)
- [WPF .NET 8 Documentation](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/)

## 👥 Equipo

- **Frontend:** Juanchi (branch: `frontend-juanchi`)
- **Repository:** [Sistema-Gestion-Uniformes](https://github.com/JaviRodrigue/Sistema-Gestion-Uniformes)

---

**Versión:** 1.0.0  
**Última actualización:** 2024
