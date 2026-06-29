namespace SistemaTransporteInterurbano.Mobile.Models;

public class ChoferDto
{
    public int ChoferId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;

    public string NombreCompleto => $"{Nombre} {Apellidos}".Trim();
}