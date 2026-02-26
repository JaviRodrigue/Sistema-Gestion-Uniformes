using System.Windows.Controls;
using VentasApp.Desktop.ViewModels.Ventas;

namespace VentasApp.Desktop.Views.Ventas;

public partial class VentaView : UserControl
{
    public VentaView(VentaViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}
