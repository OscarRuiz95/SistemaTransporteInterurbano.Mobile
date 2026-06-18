using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaTransporteInterurbano.Mobile.Models;

public class DetalleViajeDto
{
    public int ViajeId { get; set; }

    public string Ruta { get; set; } = string.Empty;

    public string PlacaUnidad { get; set; } = string.Empty;

    public string Chofer { get; set; } = string.Empty;

    public DateTime FechaSalida { get; set; }

    public DateTime FechaLlegadaEstimada { get; set; }

    public int NumeroAsiento { get; set; }

    public decimal PrecioBase { get; set; }

    public decimal MontoPagado { get; set; }

    public string Estado { get; set; } = string.Empty;
}