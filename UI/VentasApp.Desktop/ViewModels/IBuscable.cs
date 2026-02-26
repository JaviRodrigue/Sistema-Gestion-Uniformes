namespace VentasApp.Desktop.ViewModels;

public interface IBuscable
{
    Task BuscarAsync(string texto);
    Task RestablecerAsync();
}
