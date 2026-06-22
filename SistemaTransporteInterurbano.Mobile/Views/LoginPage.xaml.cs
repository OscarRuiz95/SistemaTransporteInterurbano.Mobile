using SistemaTransporteInterurbano.Mobile.Services;

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

            Preferences.Set("UsuarioId", usuario.UsuarioId);
            Preferences.Set("PasajeroId", usuario.PasajeroId);
            Preferences.Set("NombreUsuario", usuario.NombreUsuario);
            Preferences.Set("NombreCompleto", usuario.NombreCompleto);

            await DisplayAlert(
                "Bienvenido",
                $"Hola, {usuario.NombreCompleto}",
                "Continuar");

            // Luego aquí navegaremos a MisReservasPage.
            // Por ahora solo confirmamos que el login funciona.
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