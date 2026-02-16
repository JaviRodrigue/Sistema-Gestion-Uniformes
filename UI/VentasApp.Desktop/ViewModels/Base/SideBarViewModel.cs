using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public class SidebarViewModel : ObservableObject
{
    public string NombreNegocio => "Luximel";

    public IRelayCommand IrVentasCommand { get; }
    public IRelayCommand IrProductosCommand { get; }
    public IRelayCommand IrCategoriasCommand { get; }
    public IRelayCommand IrStockCommand { get; }
    public IRelayCommand IrPagosCommand { get; }

    public SidebarViewModel(INavigationService navigation)
    {
        IrVentasCommand = new RelayCommand(() => navigation.Navigate<VentaViewModel>());
        IrProductosCommand = new RelayCommand(() => navigation.Navigate<ProductoViewModel>());
        IrCategoriasCommand = new RelayCommand(() => navigation.Navigate<CategoriaViewModel>());
        IrStockCommand = new RelayCommand(() => navigation.Navigate<StockViewModel>());
        IrPagosCommand = new RelayCommand(() => navigation.Navigate<PagoViewModel>());
    }
}
