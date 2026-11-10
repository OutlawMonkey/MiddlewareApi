using WebApiNCapas.Business;
using WebApiNCapas.Data;

var builder = WebApplication.CreateBuilder(args);

// ===================================================================
// Inyección de Dependencias (DI) para Arquitectura N-Capas
// ===================================================================

// 1. Fábrica de Conexiones ADO.NET (Capa Data)
builder.Services.AddSingleton<DbConnectionFactory>();

// 2. Repositorios de Acceso a Datos (Capa Data)
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();

// 3. Servicios con Reglas de Negocio (Capa Business)
builder.Services.AddScoped<IProductoService, ProductoService>();

// 4. Controladores y Documentación OpenAPI
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configuración de la canalización de peticiones HTTP (Middleware Pipeline)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
