//using Android.Provider;
using Microsoft.Maui.Platform;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Models
{
    public class ApiService
    {
        public async Task<Root> GetWeatherByCoord(double lat, double lon)
        {
            bool isImpi = Preferences.Get("UnitPreference", false);
            var lang = Preferences.Get("SelectedOption", "en");

            string unit = "";
            string savedLang = lang;

            if(isImpi == true)
            {
                unit = "imperial";
            }
            else
            {
                unit = "metric";
            }

            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync
                ($"https://api.openweathermap.org/data/2.5/forecast?lat={lat}&lon={lon}&units={unit}&lang={savedLang}&appid=eb3a2b9461db6f822ad044664d7ab398");

            return JsonConvert.DeserializeObject<Root>(response);
        }

        public async Task<Root> GetWeatherByCity(string city)
        {
            bool isImpi = Preferences.Get("UnitPreference", false);
            var lang = Preferences.Get("SelectedOption", "en");

            string unit = "";
            string savedLang = lang;

            if (isImpi == true)
            {
                unit = "imperial";
            }
            else
            {
                unit = "metric";
            }

            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(
                    $"https://api.openweathermap.org/data/2.5/forecast?q={city}&units={unit}&lang={savedLang}&appid=eb3a2b9461db6f822ad044664d7ab398"
                );

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<Root>(jsonResponse);
                    return result;
                }
                else
                {
                    // Város nem található vagy egyéb hiba
                    await Application.Current.MainPage.DisplayAlert(
                        "City not found",
                        $"The data of '{city}' was not found. Check the spelling for mistakes.",
                        "OK"
                    );
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Általános hibakezelés
                await Application.Current.MainPage.DisplayAlert(
                    "Out of calls",
                    "I ran out of API calls, try again later :(",
                    "OK"
                );
                return null;
            }
        }
    }
}
