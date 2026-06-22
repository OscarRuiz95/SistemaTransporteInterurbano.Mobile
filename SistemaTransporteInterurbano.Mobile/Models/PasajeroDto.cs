namespace SistemaTransporteInterurbano.Mobile.Models;

public class PasajeroDto
{
    public int PasajeroId { get; set; }
    public string Identificacion { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
    public int UsuarioId { get; set; }
}
