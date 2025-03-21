//using WeatherApp.Service;
using WeatherApp.Models;

namespace WeatherApp;

public partial class ClickedOnFavorite : ContentPage
{
    public List<WeatherApp.Models.List> weatherList;
    private string cityname { get; set; }
    public ClickedOnFavorite(string cityname)
	{
        InitializeComponent();
        this.cityname = cityname;
        weatherList = new List<Models.List>();
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await GetWeatherBYCity(cityname);
    }

    

    //private async void TapLocation_Tapped(object sender, TappedEventArgs e)
    //{
    //    await GetWeatherBYCity(cityname);
    //}

    public async Task GetWeatherBYCity(string city)
    {
        ApiService apiService = new ApiService();
        var result = await apiService.GetWeatherByCity(city);

        UpdateWeather(result);
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        var response = await DisplayPromptAsync("Search by city", "Search by city", placeholder: "Start typing...", accept: "Search", cancel: "Cancel");
        if (response != null)
        {
            await GetWeatherBYCity(response);
        }
    }

    public void UpdateWeather(dynamic result)
    {
        bool isImpi = Preferences.Get("UnitPreference", false);
        weatherList.Clear();

        if(result != null)
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

    private void Button_Clicked(object sender, EventArgs e)
    {
        string cityName = labelCity.Text;

        var favorites = Preferences.Get("Favorites", string.Empty);
        var favList = string.IsNullOrEmpty(favorites) ? new List<string>() : favorites.Split(",").ToList();

        if (favList.Contains(cityName))
        {
            favList.Remove(cityName);
            Preferences.Set("Favorites", string.Join(",", favList));
            DisplayAlert("Success", $"{cityName} removed from favorites.", "OK");
        }
        else
        {
            DisplayAlert("Info", $"{cityName} is not in favorites.", "OK");
        }
    }
}