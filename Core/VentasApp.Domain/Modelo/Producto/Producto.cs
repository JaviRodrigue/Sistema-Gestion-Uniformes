namespace VentasApp.Domain.Modelo.Producto;
using VentasApp.Domain.Base;
using VentasApp.Domain.Modelo.Categoria;
public class Producto : Entidad
{
    public int IdCategoria {get; private set;}
    public string Nombre {get; private set;} = null!;
    public decimal Costo {get; private set;}
    public decimal PrecioVenta {get; private set;}
    public decimal Ganancia => PrecioVenta - Costo;
    public bool Activo;
    private readonly List<ItemVendible> _itemVendibles = new();
    public IReadOnlyCollection<ItemVendible> ItemsVendibles() => _itemVendibles.AsReadOnly();

    protected Producto(){}
    public Producto(int id_categoria,string nombre, decimal costo, decimal precioVenta)
    {
        this.IdCategoria = id_categoria;
        CambiarNombre(nombre);
        CambiarCosto(costo);
        CambiarPrecioVenta(precioVenta);
        this.Activo = true;
    }

    public void CambiarNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ExcepcionDominio("El nombre debe ser obligatorio");
        }
        Nombre = nombre.Trim();
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
        if(precioVenta < this.Costo)
        {
            throw new ExcepcionDominio("El precio venta no puede ser menor que el costo");
        }
        this.PrecioVenta = precioVenta;
    }

    public void AgregarItem(ItemVendible item)
    {
        if(_itemVendibles.Any(i => i.CodigoBarra == item.CodigoBarra))
        {
            throw new ExcepcionDominio("Ya existe un item con ese codigo de barra");
        }
        _itemVendibles.Add(item);
    }

    public void Desactivar()
    {
        this.Activo = false;
    }
    
}