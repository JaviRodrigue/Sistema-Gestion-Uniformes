namespace VentasApp.Application.DTOs.Categoria;

public class CrearCategoriaDto
{
    //Tuve que poner =null! porque me tiraba advertencia de posibles referencias a null
    public string Nombre{get; set;} = null!;
}