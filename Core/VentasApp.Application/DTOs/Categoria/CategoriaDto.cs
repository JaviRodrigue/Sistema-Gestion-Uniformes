namespace VentasApp.Application.DTOs.Categoria;

public class CategoriaDto
{
    //Tuve que poner =null! porque me tiraba advertencia de posibles referencias a null
    public int Id{get;set;}
    public string Nombre{get;set;} = null!;
    public bool Activa{get;set;}
}