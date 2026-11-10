namespace WebApiNCapas.Entities;

/// <summary>
/// Estructura genérica estándar para las respuestas de la API.
/// Ayuda a los estudiantes a devolver respuestas uniformes (éxito, mensaje, datos).
/// </summary>
public class ApiResponse<T>
{
    public bool Exito { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public T? Datos { get; set; }

    public static ApiResponse<T> Ok(T datos, string mensaje = "Operación realizada con éxito")
    {
        return new ApiResponse<T>
        {
            Exito = true,
            Mensaje = mensaje,
            Datos = datos
        };
    }

    public static ApiResponse<T> Error(string mensaje)
    {
        return new ApiResponse<T>
        {
            Exito = false,
            Mensaje = mensaje,
            Datos = default
        };
    }
}
