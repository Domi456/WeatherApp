using System.Diagnostics;
using WeatherApp.Models;

namespace WeatherApp;

public partial class FavsPage : ContentPage
{
	public FavsPage()
	{
        InitializeComponent();

        // Olvassuk be a kedvenceket
        var favorites = Preferences.Get("Favorites", string.Empty);
        var favoritesList = string.IsNullOrEmpty(favorites)
            ? new List<string>()
            : favorites.Split(',').ToList();

        if(favoritesList.Count == 0)
        {
            cityLabel.Text = "No favorites so far";
        }
        else
        {
           Favlist.ItemsSource = favoritesList;
        }
    }

    private async void Favlist_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        string cityname = e.Item.ToString();
        await Navigation.PushAsync(new ClickedOnFavorite(cityname));
    }
}