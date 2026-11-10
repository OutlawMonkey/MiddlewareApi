using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace WebApiNCapas.Data;

/// <summary>
/// Fábrica de conexiones a SQL Server usando ADO.NET puro.
/// Obtiene la cadena de conexión desde appsettings.json.
/// </summary>
public class DbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'DefaultConnection' en la configuración.");
    }

    /// <summary>
    /// Crea y devuelve una nueva instancia de SqlConnection.
    /// La responsabilidad de abrirla y cerrarla con 'using' / 'await using' es del repositorio.
    /// </summary>
    public SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
