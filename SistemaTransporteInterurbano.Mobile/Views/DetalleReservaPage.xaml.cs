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
                await DisplayAlert("Sesión inválida",
                    "Debe iniciar sesión nuevamente.",
                    "Aceptar");

                await Shell.Current.GoToAsync("//LoginPage");
                return;
            }

            var detalle = await _apiService.ObtenerDetalleAsync(
                _reserva.ViajeId,
                pasajeroId);

            if (detalle == null)
            {
                await DisplayAlert(
                    "Error",
                    "No se pudo cargar el detalle.",
                    "Aceptar");

                return;
            }

            RutaLabel.Text =
                $"Ruta: {detalle.Ruta?.Nombre}";

            EstadoLabel.Text =
                $"Estado: {detalle.EstadoTexto}";

            OrigenLabel.Text =
                $"Origen: {detalle.Ruta?.Origen}";

            DestinoLabel.Text =
                $"Destino: {detalle.Ruta?.Destino}";

            UnidadLabel.Text =
                $"Unidad: {detalle.Unidad?.Placa} - {detalle.Unidad?.Modelo}";

            ChoferLabel.Text =
                $"Chofer: {detalle.Chofer?.NombreCompleto}";

            FechaSalidaLabel.Text =
                $"Salida: {detalle.FechaSalida:dd/MM/yyyy HH:mm}";

            FechaLlegadaLabel.Text =
                $"Llegada estimada: {detalle.FechaLlegadaEstimada:dd/MM/yyyy HH:mm}";

            AsientoLabel.Text =
                $"Número de asiento: {_reserva.NumeroAsiento}";

            decimal precioBase =
                detalle.PrecioBase > 0
                    ? detalle.PrecioBase
                    : detalle.Ruta?.PrecioBase ?? 0;

            decimal montoPagado =
                detalle.MontoPagado > 0
                    ? detalle.MontoPagado
                    : _reserva.MontoPagado;

            PrecioBaseLabel.Text =
                $"Precio base: ₡{precioBase:N2}";

            MontoPagadoLabel.Text =
                $"Monto total pagado: ₡{montoPagado:N2}";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Aceptar");
        }
    }
}