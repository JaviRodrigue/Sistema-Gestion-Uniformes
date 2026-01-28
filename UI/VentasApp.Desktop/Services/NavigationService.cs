using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace VentasApp.Desktop.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;

        public event Action<ObservableObject>? OnNavigate;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void NavigateTo<TViewModel>() where TViewModel : ObservableObject
        {
            var viewModel = _serviceProvider.GetRequiredService<TViewModel>();
            OnNavigate?.Invoke(viewModel);
        }
    }
}
