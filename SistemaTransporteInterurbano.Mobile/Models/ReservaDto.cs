using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaTransporteInterurbano.Mobile.Models;

public class ReservaDto
{
    public int ReservaId { get; set; }

    public int ViajeId { get; set; }

    public string Ruta { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;

    public int NumeroAsiento { get; set; }

    public DateTime FechaSalida { get; set; }

    public decimal MontoPagado { get; set; }
}