using SistemaTransporteInterurbano.Mobile.Models;
using SistemaTransporteInterurbano.Mobile.Services;
using System.Collections.ObjectModel;

namespace SistemaTransporteInterurbano.Mobile.Views;

public partial class MisReservasPage : ContentPage
{
    private readonly ApiService _apiService;
    private readonly ObservableCollection<ReservaDto> _reservas = new();

    public MisReservasPage()
    {
        InitializeComponent();

        _apiService = new ApiService();
        ReservasCollection.ItemsSource = _reservas;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarReservasAsync();
    }

    private async Task CargarReservasAsync()
    {
        try
        {
            _reservas.Clear();

            int pasajeroId = Preferences.Get("PasajeroId", 0);

            if (pasajeroId == 0)
            {
                await DisplayAlert("Sesión inválida", "Debe iniciar sesión nuevamente.", "Aceptar");
                await Shell.Current.GoToAsync("//LoginPage");
                return;
            }

            var reservas = await _apiService.ObtenerReservasAsync(pasajeroId);

            foreach (var reserva in reservas)
                _reservas.Add(reserva);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Aceptar");
        }
    }

    private async void OnReservaSeleccionada(object sender, SelectionChangedEventArgs e)
    {
        var reserva = e.CurrentSelection.FirstOrDefault() as ReservaDto;

        if (reserva == null)
            return;

        ReservasCollection.SelectedItem = null;

        await Navigation.PushAsync(new DetalleReservaPage(reserva));
    }
}