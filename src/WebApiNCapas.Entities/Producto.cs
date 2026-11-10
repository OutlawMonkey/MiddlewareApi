namespace WebApiNCapas.Entities;

/// <summary>
/// Representa la entidad Producto (POCO - Plain Old CLR Object).
/// Esta clase es transversal y sirve como modelo de datos entre capas.
/// </summary>
public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public DateTime FechaCreacion { get; set; }
}
