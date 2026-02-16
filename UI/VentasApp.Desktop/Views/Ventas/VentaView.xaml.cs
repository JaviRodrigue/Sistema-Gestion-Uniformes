using System.Windows.Controls;
using VentasApp.Desktop.ViewModels.Ventas;
using VentasApp.Desktop.ViewModels.Ventas.VentaViewModel;

namespace VentasApp.Desktop.Views.Ventas;

public partial class VentaView : UserControl
{
    public VentaView(VentaViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}
