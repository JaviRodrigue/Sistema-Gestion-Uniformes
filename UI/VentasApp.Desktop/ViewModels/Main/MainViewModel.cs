using CommunityToolkit.Mvvm.ComponentModel;
using VentasApp.Desktop.Services;
using VentasApp.Desktop.ViewModels.Base;
using VentasApp.Desktop.ViewModels.Categorias;

namespace VentasApp.Desktop.ViewModels.Main;

public class MainViewModel : ObservableObject
{
    private readonly NavigationService _navigation;

    private ObservableObject _currentViewModel = null!;
    public ObservableObject CurrentViewModel
    {
        get => _currentViewModel;
        set => SetProperty(ref _currentViewModel, value);
    }

    public SidebarViewModel SidebarViewModel { get; }

    public MainViewModel(NavigationService navigation)
    {
        _navigation = navigation;
        SidebarViewModel = new SidebarViewModel(_navigation);

        _navigation.OnNavigate += vm => CurrentViewModel = vm;

        // Por defecto mostramos el primer ViewModel
        _navigation.NavigateTo<CategoriaViewModel>();
    }
}
