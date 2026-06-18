using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaTransporteInterurbano.Mobile.Models;

public class LoginRequest
{
    public string NombreUsuario { get; set; } = string.Empty;
    public string Clave { get; set; } = string.Empty;
}