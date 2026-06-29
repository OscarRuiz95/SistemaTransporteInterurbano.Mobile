namespace SistemaTransporteInterurbano.Mobile.Models;

public class ReservaDto
{
    public int ReservaId { get; set; }
    public int ViajeId { get; set; }
    public int NumeroAsiento { get; set; }
    public decimal MontoPagado { get; set; }

    public ViajeDto? Viaje { get; set; }

    public string RutaTexto =>
        Viaje?.Ruta != null
            ? $"{Viaje.Ruta.Origen} - {Viaje.Ruta.Destino}"
            : "Ruta no disponible";

    public string EstadoTexto =>
        Viaje?.EstadoTexto ?? "Sin estado";

    public DateTime FechaSalidaTexto =>
        Viaje?.FechaSalida ?? DateTime.MinValue;
}