using System.Windows.Controls;

namespace VentasApp.Desktop.Views.Cliente
{
    public partial class ClienteView : UserControl
    {
        public ClienteView(VentasApp.Desktop.ViewModels.Cliente.ClienteViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}