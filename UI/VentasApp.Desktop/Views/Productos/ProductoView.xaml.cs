namespace VentasApp.Desktop.Views.Productos
{
    public partial class ProductoView : System.Windows.Controls.UserControl
    {
        public ProductoView(VentasApp.Desktop.ViewModels.Productos.ProductoViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}