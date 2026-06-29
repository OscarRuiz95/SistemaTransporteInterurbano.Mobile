namespace SistemaTransporteInterurbano.Mobile.Models;

public class RutaDto
{
    public int RutaId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Origen { get; set; } = string.Empty;
    public string Destino { get; set; } = string.Empty;
    public decimal PrecioBase { get; set; }
}