using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace VentasApp.Desktop.ViewModels.Base;

public class SidebarItemViewModel : ObservableObject
{
    public string Title { get; }
    public string Icon { get; }
    public IRelayCommand Command { get; }

    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }

    public SidebarItemViewModel(string title, string icon, IRelayCommand command)
    {
        Title = title;
        Icon = icon;
        Command = command;
    }
}
