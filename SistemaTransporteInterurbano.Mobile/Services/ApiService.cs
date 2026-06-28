using System.Net.Http.Json;
using System.Text.Json;
using SistemaTransporteInterurbano.Mobile.Models;

namespace SistemaTransporteInterurbano.Mobile.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;

    private const string ApiKey = "STI-2024-SECURE-KEY";// Reemplaza con tu clave real

    // Para Windows local:
    //private const string BaseUrl = "https://localhost:7230";// revisar
    private const string BaseUrl = "https://sistematransporteinterurbano.azurewebsites.net";

    // Si luego pruebas en Android Emulator, cambia por:
    // private const string BaseUrl = "https://10.0.2.2:7230";// revisar

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

    public async Task<LoginResponse?> LoginAsync(string nombreUsuario, string clave)
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
    }

    public async Task<List<ReservaDto>> ObtenerReservasAsync(int pasajeroId)
    {
        var response = await _httpClient.GetAsync($"/api/viajes/reservas-pasajero/{pasajeroId}");

        if (!response.IsSuccessStatusCode)
            throw new Exception("No se pudieron cargar las reservas.");

        var reservas = await response.Content.ReadFromJsonAsync<List<ReservaDto>>();

        return reservas ?? new List<ReservaDto>();
    }

    public async Task<DetalleViajeDto?> ObtenerDetalleAsync(int viajeId, int pasajeroId)
    {
        var response = await _httpClient.GetAsync($"/api/viajes/{viajeId}/detalle?pasajeroId={pasajeroId}");

        if (!response.IsSuccessStatusCode)
            throw new Exception("No se pudo cargar el detalle del viaje.");

        return await response.Content.ReadFromJsonAsync<DetalleViajeDto>();
    }
}