namespace SistemaTransporteInterurbano.Mobile.Models;

public class ViajeDto
{
    public int ViajeId { get; set; }

    public RutaDto? Ruta { get; set; }
    public UnidadDto? Unidad { get; set; }
    public ChoferDto? Chofer { get; set; }

    public DateTime FechaSalida { get; set; }
    public DateTime FechaLlegadaEstimada { get; set; }

    public int Estado { get; set; }

    public string EstadoTexto => Estado switch
    {
        0 => "Programado",
        1 => "En Curso",
        2 => "Completado",
        3 => "Cancelado",
        _ => "Desconocido"
    };
}