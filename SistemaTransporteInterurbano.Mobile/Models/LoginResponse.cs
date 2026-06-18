using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaTransporteInterurbano.Mobile.Models;

public class LoginResponse
{
    public int UsuarioId { get; set; }
    public int PasajeroId { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
}