using System.Data;
using Microsoft.Data.SqlClient;
using WebApiNCapas.Entities;

namespace WebApiNCapas.Data;

/// <summary>
/// Implementación de IProductoRepository con ADO.NET NATIVO (sin ORM / sin Entity Framework).
/// Ideal para fines didácticos:
/// 1. Muestra la apertura explícita de conexiones (SqlConnection).
/// 2. Configura comandos hacia Procedimientos Almacenados (SqlCommand con CommandType.StoredProcedure).
/// 3. Asigna parámetros con tipos seguros (SqlParameter) para evitar SQL Injection.
/// 4. Mapea manualmente las filas de resultados (SqlDataReader) hacia objetos C#.
/// </summary>
public class ProductoRepository : IProductoRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public ProductoRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    /// <summary>
    /// Ejecuta el SP 'sp_ObtenerProductos' y lee fila por fila mediante SqlDataReader.
    /// </summary>
    public async Task<IEnumerable<Producto>> ObtenerTodosAsync()
    {
        var lista = new List<Producto>();

        // 'await using' asegura que la conexión se cierre y libere de la memoria al terminar el bloque
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        // Creamos el comando indicando el nombre del Procedimiento Almacenado
        await using var command = new SqlCommand("sp_ObtenerProductos", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        // Ejecutamos la lectura
        await using var reader = await command.ExecuteReaderAsync();

        // Leemos cada registro mientras reader.ReadAsync() devuelva true
        while (await reader.ReadAsync())
        {
            lista.Add(MapearProductoDesdeReader(reader));
        }

        return lista;
    }

    /// <summary>
    /// Ejecuta el SP 'sp_ObtenerProductoPorId' enviando el parámetro @Id.
    /// </summary>
    public async Task<Producto?> ObtenerPorIdAsync(int id)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        await using var command = new SqlCommand("sp_ObtenerProductoPorId", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        // Pasamos el parámetro @Id de forma tipada
        command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });

        await using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return MapearProductoDesdeReader(reader);
        }

        return null;
    }

    /// <summary>
    /// Ejecuta el SP 'sp_CrearProducto' y obtiene el nuevo Id generado usando ExecuteScalarAsync.
    /// </summary>
    public async Task<int> CrearAsync(Producto producto)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        await using var command = new SqlCommand("sp_CrearProducto", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        // Mapeo explícito de parámetros (Nombre, Descripcion, Precio, Stock)
        command.Parameters.Add(new SqlParameter("@Nombre", SqlDbType.NVarChar, 100) { Value = producto.Nombre });
        command.Parameters.Add(new SqlParameter("@Descripcion", SqlDbType.NVarChar, 250) 
        { 
            Value = (object?)producto.Descripcion ?? DBNull.Value 
        });
        command.Parameters.Add(new SqlParameter("@Precio", SqlDbType.Decimal) { Precision = 10, Scale = 2, Value = producto.Precio });
        command.Parameters.Add(new SqlParameter("@Stock", SqlDbType.Int) { Value = producto.Stock });

        // ExecuteScalarAsync se usa cuando el SP retorna un único valor escalar (ej: SELECT SCOPE_IDENTITY())
        var resultado = await command.ExecuteScalarAsync();
        return Convert.ToInt32(resultado);
    }

    /// <summary>
    /// Ejecuta el SP 'sp_ActualizarProducto' y verifica si se afectó al menos una fila.
    /// </summary>
    public async Task<bool> ActualizarAsync(Producto producto)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        await using var command = new SqlCommand("sp_ActualizarProducto", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = producto.Id });
        command.Parameters.Add(new SqlParameter("@Nombre", SqlDbType.NVarChar, 100) { Value = producto.Nombre });
        command.Parameters.Add(new SqlParameter("@Descripcion", SqlDbType.NVarChar, 250) 
        { 
            Value = (object?)producto.Descripcion ?? DBNull.Value 
        });
        command.Parameters.Add(new SqlParameter("@Precio", SqlDbType.Decimal) { Precision = 10, Scale = 2, Value = producto.Precio });
        command.Parameters.Add(new SqlParameter("@Stock", SqlDbType.Int) { Value = producto.Stock });

        var resultado = await command.ExecuteScalarAsync();
        int filasAfectadas = Convert.ToInt32(resultado);
        return filasAfectadas > 0;
    }

    /// <summary>
    /// Ejecuta el SP 'sp_EliminarProducto'.
    /// </summary>
    public async Task<bool> EliminarAsync(int id)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        await using var command = new SqlCommand("sp_EliminarProducto", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });

        var resultado = await command.ExecuteScalarAsync();
        int filasAfectadas = Convert.ToInt32(resultado);
        return filasAfectadas > 0;
    }

    /// <summary>
    /// Método auxiliar didáctico: muestra a los estudiantes cómo leer cada columna
    /// de un SqlDataReader y verificar si contiene DBNull.
    /// </summary>
    private static Producto MapearProductoDesdeReader(SqlDataReader reader)
    {
        return new Producto
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
            Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) 
                ? null 
                : reader.GetString(reader.GetOrdinal("Descripcion")),
            Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
            Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
            FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion"))
        };
    }
}
