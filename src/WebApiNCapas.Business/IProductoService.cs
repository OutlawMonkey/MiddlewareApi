using WebApiNCapas.Entities;

namespace WebApiNCapas.Business;

/// <summary>
/// Contrato del servicio de negocio para Producto.
/// </summary>
public interface IProductoService
{
    Task<IEnumerable<Producto>> ListarAsync();
    Task<Producto?> BuscarPorIdAsync(int id);
    Task<int> RegistrarAsync(Producto producto);
    Task<bool> ModificarAsync(int id, Producto producto);
    Task<bool> DarDeBajaAsync(int id);
}
