using Microsoft.Extensions.DependencyInjection;

public class NavigationService : INavigationService
{
    private readonly IServiceProvider _provider;
    private readonly MainViewModel _main;

    public NavigationService(IServiceProvider provider, MainViewModel main)
    {
        _provider = provider;
        _main = main;
    }

    public void Navigate<TViewModel>() where TViewModel : class
    {
        _main.CurrentViewModel = _provider.GetRequiredService<TViewModel>();
    }
}
