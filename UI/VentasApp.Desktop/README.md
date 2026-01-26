```text
VentasApp.Desktop
│
├── App.xaml
├── App.xaml.cs
│
├── Bootstrap/
│   ├── DependencyInjection.cs
│   ├── NavigationService.cs
│
├── Views/
│   ├── Main/
│   │   ├── MainWindow.xaml
│   │   └── MainWindow.xaml.cs
│   │
│   ├── Venta/
│   │   ├── VentaView.xaml
│   │   ├── VentaDetalleView.xaml
│   │   └── PagoView.xaml
│   │
│   ├── Producto/
│   │   ├── ProductoView.xaml
│   │   ├── ItemVendibleView.xaml
│   │   └── StockView.xaml
│   │
│   ├── Categoria/
│   │   └── CategoriaView.xaml
│
├── ViewModels/
│   ├── Base/
│   │   └── ViewModelBase.cs
│   │
│   ├── Main/
│   │   └── MainViewModel.cs
│   │
│   ├── Venta/
│   │   ├── VentaViewModel.cs
│   │   ├── VentaDetalleViewModel.cs
│   │   └── PagoViewModel.cs
│   │
│   ├── Producto/
│   │   ├── ProductoViewModel.cs
│   │   ├── ItemVendibleViewModel.cs
│   │   └── StockViewModel.cs
│   │
│   ├── Categoria/
│   │   └── CategoriaViewModel.cs
│
├── Commands/
│   └── RelayCommand.cs (si no usás Toolkit)
│
├── Converters/
│   ├── BoolToVisibilityConverter.cs
│   └── EstadoVentaConverter.cs
│
├── Services/
│   ├── DialogService.cs
│   ├── MessageService.cs
│   └── NavigationService.cs
│
├── Resources/
│   ├── Styles/
│   │   ├── Colors.xaml
│   │   ├── Buttons.xaml
│   │   └── TextBoxes.xaml
│   │
│   └── Templates/
│       └── VentaTemplates.xaml
│
├── Mapping/
│   └── ViewModelMapper.cs
│
├── Models/
│   └── ViewModelsDTOs.cs
│
└── appsettings.json

```


### Flujo del UI al Backend
```text
View (XAML)
 ↓
ViewModel
 ↓
UseCase (Application)
 ↓
Repository
 ↓
EF Core

```