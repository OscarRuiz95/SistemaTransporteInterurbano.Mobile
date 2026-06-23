using SistemaTransporteInterurbano.Mobile.Models;
using SistemaTransporteInterurbano.Mobile.Services;

namespace SistemaTransporteInterurbano.Mobile.Views;

public partial class DetalleReservaPage : ContentPage
{
    private readonly ApiService _apiService;
    private readonly ReservaDto _reserva;

    public DetalleReservaPage(ReservaDto reserva)
    {
        InitializeComponent();

        _apiService = new ApiService();
        _reserva = reserva;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarDetalleAsync();
    }

    private async Task CargarDetalleAsync()
    {
        try
        {
            int pasajeroId = Preferences.Get("PasajeroId", 0);

            if (pasajeroId == 0)
            {
                await DisplayAlert("Sesión inválida", "Debe iniciar sesión nuevamente.", "Aceptar");
                await Shell.Current.GoToAsync("//LoginPage");
                return;
            }

            var detalle = await _apiService.ObtenerDetalleAsync(_reserva.ViajeId, pasajeroId);

            if (detalle == null)
            {
                await DisplayAlert("Error", "No se pudo cargar el detalle del viaje.", "Aceptar");
                return;
            }

            RutaLabel.Text = $"Ruta: {detalle.Ruta}";
            PlacaLabel.Text = $"Placa de unidad: {detalle.PlacaUnidad}";
            ChoferLabel.Text = $"Chofer: {detalle.Chofer}";
            FechaSalidaLabel.Text = $"Salida: {detalle.FechaSalida:dd/MM/yyyy HH:mm}";
            FechaLlegadaLabel.Text = $"Llegada estimada: {detalle.FechaLlegadaEstimada:dd/MM/yyyy HH:mm}";
            AsientoLabel.Text = $"Asiento asignado: {detalle.NumeroAsiento}";

            PrecioBaseLabel.Text = $"Precio base: ₡{detalle.PrecioBase:N2}";
            MontoPagadoLabel.Text = $"Monto total pagado: ₡{detalle.MontoPagado:N2}";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Aceptar");
        }
    }
}