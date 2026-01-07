namespace VentasApp.Domain.Modelo.Producto;
using VentasApp.Domain.Base;
using VentasApp.Domain.Modelo.Categoria;
public class Producto : Entidad
{
    public int IdCategoria {get; private set;}
    public string Nombre {get; private set;}
    public decimal Costo {get; private set;}
    public decimal PrecioVenta {get; private set;}
    public decimal Ganancia => PrecioVenta - Costo;
    public Categoria Categoria {get;}
    private readonly List<ItemVendible> _itemVendibles = new();

    public IReadOnlyCollection<ItemVendible> ItemsVendibles() => _itemVendibles.AsReadOnly();

    public Producto(int id_categoria,string nombre, decimal costo, decimal precioVenta)
    {
        this.IdCategoria = id_categoria;
        CambiarNombre(nombre);
        CambiarCosto(costo);
        CambiarPrecioVenta(precioVenta);
    }

    public void CambiarNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ExcepcionDominio("El nombre debe ser obligatorio");
        }
        Nombre = nombre.Trim();
        Nombre = Nombre.ToLower();
    }

    public void CambiarCosto(decimal costo)
    {
        if (costo <= 0)
        {
            throw new ExcepcionDominio("El costo debe ser mayor a 0");
        }
        this.Costo = costo;
    }

    public void CambiarPrecioVenta(decimal precioVenta)
    {
        if(precioVenta <= 0)
        {
            throw new ExcepcionDominio("El precio venta debe ser mayor a 0");
        }
    }

    public void AgregarItem(ItemVendible item)
    {
        _itemVendibles.Add(item);
    }

    
}