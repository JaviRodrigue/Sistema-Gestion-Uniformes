using CommunityToolkit.Mvvm.ComponentModel;

public class MainViewModel : ObservableObject
{
    public SidebarViewModel SidebarViewModel { get; }

    private object? _currentViewModel;
    public object? CurrentViewModel
    {
        get => _currentViewModel;
        set => SetProperty(ref _currentViewModel, value);
    }

    public MainViewModel(SidebarViewModel sidebar)
    {
        SidebarViewModel = sidebar;
    }
}
