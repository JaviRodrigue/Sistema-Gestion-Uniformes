using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VentasApp.Desktop.Views.Ventas;
using VentasApp.Desktop.Views.Cliente;
using VentasApp.Desktop.Views.Productos;
using VentasApp.Desktop.Converters; // Added DTO namespace import

namespace VentasApp.Desktop;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
        // Cargar vista inicial
        ContentHost.Content = new VentasApp.Desktop.Views.Productos.ProductoView();
	}

    private void OnBtnVentasClick(object sender, RoutedEventArgs e)
    {
        ContentHost.Content = new VentasApp.Desktop.Views.Ventas.VentaView(); // Adjusted to match new namespace
    }

    private void OnBtnClientesClick(object sender, RoutedEventArgs e)
    {
        ContentHost.Content = new VentasApp.Desktop.Views.Cliente.ClienteView();
    }

    private void OnBtnProductosClick(object sender, RoutedEventArgs e)
    {
        ContentHost.Content = new VentasApp.Desktop.Views.Productos.ProductoView();
    }

    private void OnBtnConfigClick(object sender, RoutedEventArgs e)
    {
        // Placeholder de configuración
        ContentHost.Content = new TextBlock { Text = "Configuración (próximamente)", FontSize = 24 };
    }
}