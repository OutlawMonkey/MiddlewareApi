using WebApiNCapas.Entities;

namespace WebApiNCapas.Data;

/// <summary>
/// Contrato del repositorio para la entidad Producto.
/// Define las operaciones de persistencia que ejecutará el Data Layer.
/// </summary>
public interface IProductoRepository
{
    Task<IEnumerable<Producto>> ObtenerTodosAsync();
    Task<Producto?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(Producto producto);
    Task<bool> ActualizarAsync(Producto producto);
    Task<bool> EliminarAsync(int id);
}
