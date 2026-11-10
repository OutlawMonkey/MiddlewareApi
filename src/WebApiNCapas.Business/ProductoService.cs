using WebApiNCapas.Data;
using WebApiNCapas.Entities;

namespace WebApiNCapas.Business;

/// <summary>
/// Capa de Lógica de Negocio (BLL - Business Logic Layer).
/// Didáctico: Aquí se aplican las reglas y validaciones antes de tocar la base de datos.
/// Los alumnos comprenderán que la base de datos no debe recibir información inválida.
/// </summary>
public class ProductoService : IProductoService
{
    private readonly IProductoRepository _repository;

    public ProductoService(IProductoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Producto>> ListarAsync()
    {
        return await _repository.ObtenerTodosAsync();
    }

    public async Task<Producto?> BuscarPorIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("El Id debe ser un entero positivo mayor a cero.");
        }

        return await _repository.ObtenerPorIdAsync(id);
    }

    public async Task<int> RegistrarAsync(Producto producto)
    {
        ValidarProducto(producto);

        return await _repository.CrearAsync(producto);
    }

    public async Task<bool> ModificarAsync(int id, Producto producto)
    {
        if (id <= 0)
        {
            throw new ArgumentException("El Id especificado no es válido.");
        }

        ValidarProducto(producto);

        // Aseguramos que el Id del objeto coincida con el de la ruta
        producto.Id = id;

        // Validamos si el producto existe antes de intentar modificarlo
        var productoExistente = await _repository.ObtenerPorIdAsync(id);
        if (productoExistente == null)
        {
            return false;
        }

        return await _repository.ActualizarAsync(producto);
    }

    public async Task<bool> DarDeBajaAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("El Id debe ser mayor a cero.");
        }

        var productoExistente = await _repository.ObtenerPorIdAsync(id);
        if (productoExistente == null)
        {
            return false;
        }

        return await _repository.EliminarAsync(id);
    }

    /// <summary>
    /// Reglas de negocio didácticas:
    /// - El nombre no puede estar en blanco.
    /// - El precio debe ser mayor a 0.
    /// - El stock no puede ser negativo.
    /// </summary>
    private static void ValidarProducto(Producto producto)
    {
        if (producto == null)
        {
            throw new ArgumentNullException(nameof(producto), "Los datos del producto no pueden ser nulos.");
        }

        if (string.IsNullOrWhiteSpace(producto.Nombre))
        {
            throw new ArgumentException("El nombre del producto es obligatorio.");
        }

        if (producto.Precio <= 0)
        {
            throw new ArgumentException("El precio del producto debe ser mayor a 0.");
        }

        if (producto.Stock < 0)
        {
            throw new ArgumentException("El stock del producto no puede ser negativo.");
        }
    }
}
