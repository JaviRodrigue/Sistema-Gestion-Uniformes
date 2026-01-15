namespace VentasApp.Application.Interfaces.Repositorios;

//Esta interfas representa una frontera transaccional
//Defina cuando se confirma una operacion completa en la base de datos
public interface IUnitOfWork
{
    Task<int> SaveChanges();
}