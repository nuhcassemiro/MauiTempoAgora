using MauiTempoAgora.Models;
using MauiTempoAgora.Service;
using System;
using System.Diagnostics;

namespace MauiTempoAgora
{
    public partial class MainPage : ContentPage
    {
        CancellationTokenSource _cancellationTokenSource;
        bool _isCheckingLocation;

        string? cidade;

       public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                _cancelTokenSource = new CancellationTokenSource();

                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                Location? Location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                if(Location != null)
                {
                    lbl_latitude.Text = Location.Latitude.ToString();
                    lbl_longitude.Text = Location.Longitude.ToString();

                    Debug.WriteLine("----------------------------------");
                    Debug.WriteLine(location);
                    Debug.WriteLine("----------------------------------");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Erro: Dispositivo não Suporta", fnsEx.Message, "OK");
            }
            catch(FeatureNotEnabledException fneEx)
            {
                await DisplayAlert("Erro: Localização Desabilitada", fneEx.Message, "OK");
            }
            catch(PermissionException pEx)
            {
                await DisplayAlert("Erro: Permissão", pEx.Message, "OK");
            }
            catch(Exception ex)
            {
                await DisplayAlert("Erro: ", ex.Message, "OK");
            }
        }//fecha método

        private async Task<string> GetGeocodeReverseData(double latitude = 47.673988, double longitude = -122.121513)
        {
            IEnumerable<Placemark> placemarks = await Geocoding.Default.GetPlacemarksAsync(latitude, longitude);

            Placemark? placemark = placemarks?.FirstOrDefault();

            Debug.WriteLine("----------------------------------");
            Debug.WriteLine(placemark?.Locality);
            Debug.WriteLine("----------------------------------");

            if (placemark != null)
            {
                cidade = placemark.Locality;

                return
                    $"AdminArea:                 {placemark.AdminArea}\n" +
                    $"CountryCode:               {placemark.CountryCode}\n" +
                    $"CountryName:               {placemark.CountryName}\n" +
                    $"FeatureName:               {placemark.FeatureName}\n" +
                    $"Locality:                  {placemark.Locality}\n" +
                    $"PostalCode:                {placemark.PostalCode}\n" +
                    $"SubAdminArea:              {placemark.SubAdminArea}\n" +
                    $"SubLocality:               {placemark.SubLocality}\n" +
                    $"SubThoroughfare:           {placemark.SubThoroughfare}\n" +
                    $"Thoroughfare:              {placemark.Thoroughfare}\n";
            }
            return "Nada";
        }
        
    }

}
