using CommunityToolkit.Mvvm.ComponentModel;

namespace VentasApp.Desktop.Services
{
    public interface INavigationService
    {
        event Action<ObservableObject>? OnNavigate;
        void NavigateTo<TViewModel>() where TViewModel : ObservableObject;
    }
}