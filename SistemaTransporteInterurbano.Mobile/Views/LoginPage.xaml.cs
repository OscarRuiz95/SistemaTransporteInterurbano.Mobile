using SistemaTransporteInterurbano.Mobile.Services;
using SistemaTransporteInterurbano.Mobile.Views;

namespace SistemaTransporteInterurbano.Mobile.Views;

public partial class LoginPage : ContentPage
{
    private readonly ApiService _apiService;

    public LoginPage()
    {
        InitializeComponent();
        _apiService = new ApiService();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        MensajeErrorLabel.IsVisible = false;

        var nombreUsuario = NombreUsuarioEntry.Text?.Trim();
        var clave = ClaveEntry.Text?.Trim();

        if (string.IsNullOrWhiteSpace(nombreUsuario) ||
            string.IsNullOrWhiteSpace(clave))
        {
            MostrarError("Debe ingresar nombre de usuario y clave.");
            return;
        }

        try
        {
            CambiarEstadoCarga(true);

            var usuario = await _apiService.LoginAsync(nombreUsuario, clave);

            if (usuario == null)
            {
                MostrarError("No se pudo iniciar sesión.");
                return;
            }

            var pasajero = await _apiService.ObtenerPasajeroPorUsuarioAsync(usuario.UsuarioId);

            if (pasajero == null)
            {
                MostrarError("El usuario no tiene un perfil de pasajero asociado.");
                return;
            }

            var nombreCompleto = $"{pasajero.Nombre} {pasajero.Apellidos}".Trim();

            Preferences.Set("UsuarioId", usuario.UsuarioId);
            Preferences.Set("PasajeroId", pasajero.PasajeroId);
            Preferences.Set("NombreUsuario", usuario.NombreUsuario ?? "");
            Preferences.Set("NombreCompleto", nombreCompleto);

            var nombreMostrar = !string.IsNullOrWhiteSpace(nombreCompleto)
                ? nombreCompleto
                : usuario.NombreUsuario;

            await DisplayAlert(
                "Bienvenido",
                $"Hola, {nombreMostrar}",
                "Continuar");

            await Navigation.PushAsync(new MisReservasPage());
        }
        catch (Exception ex)
        {
            MostrarError(ex.Message);
        }
        finally
        {
            CambiarEstadoCarga(false);
        }
    }

    private void MostrarError(string mensaje)
    {
        MensajeErrorLabel.Text = mensaje;
        MensajeErrorLabel.IsVisible = true;
    }

    private void CambiarEstadoCarga(bool cargando)
    {
        CargandoIndicator.IsRunning = cargando;
        CargandoIndicator.IsVisible = cargando;
        LoginButton.IsEnabled = !cargando;
    }
}