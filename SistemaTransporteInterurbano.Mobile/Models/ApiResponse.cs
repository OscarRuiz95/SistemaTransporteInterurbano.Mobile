namespace SistemaTransporteInterurbano.Mobile.Models;

public class ApiResponse<T>
{
    public bool Exitoso { get; set; }
    public T? Datos { get; set; }
    public string? Mensaje { get; set; }
}
