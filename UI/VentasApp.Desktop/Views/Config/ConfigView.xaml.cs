using System.Windows.Controls;
using VentasApp.Desktop.ViewModels.Config;

namespace VentasApp.Desktop.Views.Config;

public partial class ConfigView : UserControl
{
    public ConfigView(ConfigViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}
