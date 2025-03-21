namespace WeatherApp;
using WeatherApp.Models;

public partial class SettingsPage : ContentPage
{
    private const string UnitPreferenceKey = "UnitPreference";
    public SettingsPage()
	{
		InitializeComponent();
        bool isImpi = Preferences.Get(UnitPreferenceKey, false); // Alapértelmezés: false (imperial)
        unitSwitch.IsToggled = isImpi;

        var savedOption = Preferences.Get("SelectedOption", "No saved option");
        SavedOptionLabel.Text = $"Saved language: {savedOption}";

        // Ha van mentett érték, megkeressük a Pickerben és kiválasztjuk
        var index = languageDropdown.Items.IndexOf(savedOption);
        if (index != -1)
        {
            languageDropdown.SelectedIndex = index;
        }
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new WeatherPage());
    }

    private async void ImageButton_Clicked_1(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage());
    }

    private async void ImageButton_Clicked_2(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new WeatherPage());
    }

    private void unitSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set(UnitPreferenceKey, e.Value);
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        if (languageDropdown.SelectedIndex != -1)
        {
            string selectedLang = languageDropdown.SelectedItem.ToString();
            string langCode = string.Empty;

            switch (selectedLang)
            {
                case "English":
                    langCode = "en";
                    break;
                case "Deutsch":
                    langCode = "de";
                    break;
                case "Magyar":
                    langCode = "hu";
                    break;
                case "中文":
                    langCode = "zh_cn";
                    break;
                case "Español":
                    langCode = "sp";
                    break;
                case "日本語":
                    langCode = "ja";
                    break;
                case "Русский":
                    langCode = "ru";
                    break;
                default:
                    langCode = "en"; // Alapértelmezett érték
                    break;
            }

            // Elmentjük az értéket a Preferences-be
            Preferences.Set("SelectedOption", langCode);

            // Frissítjük a megjelenített mentett értéket
            SavedOptionLabel.Text = $"Saved Option: {selectedLang}";
        }
        else
        {
            SavedOptionLabel.Text = "No option selected!";
        }

        //DisplayAlert("Saved", $"Language {languageDropdown.SelectedItem} saved", "OK");
    }
}