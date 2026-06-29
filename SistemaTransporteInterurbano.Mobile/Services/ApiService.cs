using System.Net.Http.Json;
using System.Text.Json;
using SistemaTransporteInterurbano.Mobile.Models;

namespace SistemaTransporteInterurbano.Mobile.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;

    private const string ApiKey = "STI-2024-SECURE-KEY";// Reemplaza con tu clave real

   
    private const string BaseUrl = "https://sistematransporteinterurbano-api-hce6gjb9fha7erex.westus2-01.azurewebsites.net";

    
    public ApiService()
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        _httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri(BaseUrl)
        };

        _httpClient.DefaultRequestHeaders.Add("X-API-KEY", ApiKey);
    }

    /*public async Task<LoginResponse?> LoginAsync(string nombreUsuario, string clave)
    {
        var request = new LoginRequest
        {
            NombreUsuario = nombreUsuario,
            Clave = clave
        };

        var response = await _httpClient.PostAsJsonAsync("/api/autenticacion/iniciar-sesion", request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();

            try
            {
                using var json = JsonDocument.Parse(error);

                if (json.RootElement.TryGetProperty("mensaje", out var mensaje))
                    throw new Exception(mensaje.GetString());
            }
            catch (JsonException)
            {
                throw new Exception("No se pudo iniciar sesión. Verifique sus credenciales.");
            }

            throw new Exception("No se pudo iniciar sesión.");
        }

        return await response.Content.ReadFromJsonAsync<LoginResponse>();
    }*/

    public async Task<LoginResponse?> LoginAsync(string nombreUsuario, string clave)
    {
        var request = new LoginRequest
        {
            NombreUsuario = nombreUsuario,
            Clave = clave
        };

        var response = await _httpClient.PostAsJsonAsync("/api/autenticacion/iniciar-sesion", request);

        var json = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception("No se pudo iniciar sesión. Verifique sus credenciales.");

        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        if (root.TryGetProperty("datos", out var datos))
        {
            return JsonSerializer.Deserialize<LoginResponse>(
                datos.GetRawText(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        throw new Exception("La respuesta de la API no contiene datos del usuario.");
    }

    public async Task<PasajeroDto?> ObtenerPasajeroPorUsuarioAsync(int usuarioId)
    {
        var response = await _httpClient.GetAsync($"/api/pasajeros/por-usuario/{usuarioId}");

        if (!response.IsSuccessStatusCode)
            throw new Exception("No se encontró el perfil de pasajero.");

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        if (root.TryGetProperty("datos", out var datos))
        {
            return JsonSerializer.Deserialize<PasajeroDto>(
                datos.GetRawText(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        throw new Exception("No se pudo obtener el perfil de pasajero.");
    }

    public async Task<List<ReservaDto>> ObtenerReservasAsync(int pasajeroId)
    {
        var response = await _httpClient.GetAsync($"/api/viajes/reservas-pasajero/{pasajeroId}");

        if (!response.IsSuccessStatusCode)
            throw new Exception("No se pudieron cargar las reservas.");

        var json = await response.Content.ReadAsStringAsync();
      //  await Application.Current.MainPage.DisplayAlert("JSON reservas", json, "OK");//borrar mas adelante
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;
        
        if (root.TryGetProperty("datos", out var datos))
        {
            return JsonSerializer.Deserialize<List<ReservaDto>>(
                datos.GetRawText(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? new List<ReservaDto>();
        }

        return new List<ReservaDto>();
    }

    public async Task<DetalleViajeDto?> ObtenerDetalleAsync(int viajeId, int pasajeroId)
    {
        var response = await _httpClient.GetAsync($"/api/viajes/{viajeId}/detalle?pasajeroId={pasajeroId}");

        if (!response.IsSuccessStatusCode)
            throw new Exception("No se pudo cargar el detalle del viaje.");

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        if (root.TryGetProperty("datos", out var datos))
        {
            return JsonSerializer.Deserialize<DetalleViajeDto>(
                datos.GetRawText(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        return null;
    }
}