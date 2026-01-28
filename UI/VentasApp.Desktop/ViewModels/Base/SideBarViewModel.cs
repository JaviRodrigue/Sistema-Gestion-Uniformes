using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using VentasApp.Desktop.Services;
using VentasApp.Desktop.ViewModels.Productos;
using VentasApp.Desktop.ViewModels.Ventas;
using VentasApp.Desktop.ViewModels.Categorias;

namespace VentasApp.Desktop.ViewModels.Base;

public class SidebarViewModel : ObservableObject
{
    private readonly NavigationService _navigation;

    public ObservableCollection<SidebarItemViewModel> Items { get; }

    public SidebarViewModel(NavigationService navigation)
    {
        _navigation = navigation;

        Items = new ObservableCollection<SidebarItemViewModel>
        {
            new SidebarItemViewModel("Clientes", "👤", new RelayCommand(() => _navigation.NavigateTo<CategoriaViewModel>())),
            new SidebarItemViewModel("Productos", "📦", new RelayCommand(() => _navigation.NavigateTo<ProductoViewModel>())),
            new SidebarItemViewModel("Ventas", "💰", new RelayCommand(() => _navigation.NavigateTo<VentaViewModel>()))
        };
    }
}
