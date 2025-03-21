//using Metal;
using System.Formats.Asn1;
using WeatherApp.Models;

namespace WeatherApp;

public partial class WeatherPage : ContentPage
{
    public List<Models.List> weatherList;
    private double latitude { get; set; }
    private double longitude { get; set; }

	public WeatherPage()
	{
		InitializeComponent();
        weatherList = new List<Models.List>();
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await GetLocation();
        await GetWeatherBYLocation(latitude, longitude);
    }

    public async Task GetLocation()
    {
        var location = await Geolocation.GetLocationAsync();
        latitude = location.Latitude;
        longitude = location.Longitude;
    }

    private async void TapLocation_Tapped(object sender, TappedEventArgs e)
    {
        await GetLocation();
        await GetWeatherBYLocation(latitude, longitude);
    }

    public async Task GetWeatherBYLocation(double latitude, double longitude)
    {
        ApiService apiService = new ApiService();
        var result = await apiService.GetWeatherByCoord(latitude, longitude);

        UpdateWeather(result);
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        var response = await DisplayPromptAsync("Search by city", "Search by city", placeholder:"Start typing...", accept:"Search", cancel:"Cancel");
        if(response != null)
        {
            await GetWeatherBYCity(response);
        }
    }

    public async Task GetWeatherBYCity(string city)
    {
        ApiService apiService = new ApiService();
        var result = await apiService.GetWeatherByCity(city);

        UpdateWeather(result);
    }

    public void UpdateWeather(dynamic result)
    {
        bool isImpi = Preferences.Get("UnitPreference", false);
        weatherList.Clear();

        if (result != null)
        {
            for (int i = 0; i < result.list.Count / 2; i++)
            {
                weatherList.Add(result.list[i]);
            }

            labelCity.Text = result.city.name;
            labelWeatherDescription.Text = result.list[0].weather[0].description;
            if (isImpi == true)
            {
                labelTemperature.Text = result.list[0].main.temperature + "°F";
            }
            else
            {
                labelTemperature.Text = result.list[0].main.temperature + "°C";
            }
            lblHumidity.Text = result.list[0].main.humidity + "%";
            labelWind.Text = result.list[0].wind.speed + "km/h";
            ImgWheatericon.Source = result.list[0].weather[0].fullIconUrl;
            cityWeather.ItemsSource = null;
            cityWeather.ItemsSource = weatherList;
        }
    }

    private async void ImageButton_Clicked_1(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new WeatherPage());
    }

    private async void ImageButton_Clicked_2(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage());
    }

    private async void ImageButton_Clicked_3(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FavsPage());
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        string cityName = labelCity.Text;

        var favorites = Preferences.Get("Favorites", string.Empty);
        var favList = string.IsNullOrEmpty(favorites) ? new List<string>() : favorites.Split(",").ToList();

        if(!favList.Contains(cityName))
        {
            favList.Add(cityName);
            Preferences.Set("Favorites", string.Join(",", favList));
            DisplayAlert("Success", $"{cityName} added to favorites.", "OK");
        }
        else
        {
            DisplayAlert("Info", $"{cityName} is already in favorites.", "OK");
        }
    }
}