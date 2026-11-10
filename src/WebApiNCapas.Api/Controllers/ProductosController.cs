using Microsoft.AspNetCore.Mvc;
using WebApiNCapas.Business;
using WebApiNCapas.Entities;

namespace WebApiNCapas.Api.Controllers;

/// <summary>
/// Controlador de la API para la gestión de Productos.
/// Capa de Presentación: Recibe peticiones HTTP, delega la lógica a la capa Business
/// y retorna códigos de estado HTTP apropiados (200, 201, 400, 404, 500).
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _productoService;
    private readonly ILogger<ProductosController> _logger;

    public ProductosController(IProductoService productoService, ILogger<ProductosController> logger)
    {
        _productoService = productoService;
        _logger = logger;
    }

    /// <summary>
    /// GET: api/productos
    /// Obtiene la lista completa de productos.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<Producto>>>> GetAll()
    {
        try
        {
            var productos = await _productoService.ListarAsync();
            return Ok(ApiResponse<IEnumerable<Producto>>.Ok(productos));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al listar productos.");
            return StatusCode(500, ApiResponse<IEnumerable<Producto>>.Error("Error interno al obtener los productos."));
        }
    }

    /// <summary>
    /// GET: api/productos/{id}
    /// Obtiene un producto por su identificador.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<Producto>>> GetById(int id)
    {
        try
        {
            var producto = await _productoService.BuscarPorIdAsync(id);
            if (producto == null)
            {
                return NotFound(ApiResponse<Producto>.Error($"No se encontró ningún producto con el Id {id}."));
            }

            return Ok(ApiResponse<Producto>.Ok(producto));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<Producto>.Error(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar el producto {Id}.", id);
            return StatusCode(500, ApiResponse<Producto>.Error("Error interno al buscar el producto."));
        }
    }

    /// <summary>
    /// POST: api/productos
    /// Registra un nuevo producto.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<Producto>>> Create([FromBody] Producto nuevoProducto)
    {
        try
        {
            int nuevoId = await _productoService.RegistrarAsync(nuevoProducto);
            nuevoProducto.Id = nuevoId;

            return CreatedAtAction(
                nameof(GetById), 
                new { id = nuevoId }, 
                ApiResponse<Producto>.Ok(nuevoProducto, "Producto registrado con éxito.")
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<Producto>.Error(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear el producto.");
            return StatusCode(500, ApiResponse<Producto>.Error("Error interno al guardar el producto."));
        }
    }

    /// <summary>
    /// PUT: api/productos/{id}
    /// Actualiza un producto existente.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<bool>>> Update(int id, [FromBody] Producto producto)
    {
        try
        {
            bool actualizado = await _productoService.ModificarAsync(id, producto);
            if (!actualizado)
            {
                return NotFound(ApiResponse<bool>.Error($"No se encontró el producto con Id {id} para actualizar."));
            }

            return Ok(ApiResponse<bool>.Ok(true, "Producto actualizado correctamente."));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<bool>.Error(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar el producto {Id}.", id);
            return StatusCode(500, ApiResponse<bool>.Error("Error interno al actualizar el producto."));
        }
    }

    /// <summary>
    /// DELETE: api/productos/{id}
    /// Elimina un producto por su Id.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        try
        {
            bool eliminado = await _productoService.DarDeBajaAsync(id);
            if (!eliminado)
            {
                return NotFound(ApiResponse<bool>.Error($"No se encontró el producto con Id {id} para eliminar."));
            }

            return Ok(ApiResponse<bool>.Ok(true, "Producto eliminado correctamente."));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<bool>.Error(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar el producto {Id}.", id);
            return StatusCode(500, ApiResponse<bool>.Error("Error interno al eliminar el producto."));
        }
    }
}
