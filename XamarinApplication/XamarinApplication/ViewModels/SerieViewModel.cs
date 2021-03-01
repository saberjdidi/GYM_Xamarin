using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;

namespace XamarinApplication.ViewModels
{
    public class SerieViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }
        //public string AccessToken { get; set; }
        public SerieViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
        }

        public async void GetSeries()
        {
            using (var client = new HttpClient())
            {
                // send a GET request  
                var accesstoken = Settings.AccessToken;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
                var uri = "http://phoneofficine.it/niini-gim/series";
                var result = await client.GetStringAsync(uri);
                //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + result);
                //handling the answer  
                var SerieList = JsonConvert.DeserializeObject<List<Serie>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                Series = new ObservableCollection<Serie>(SerieList);
            }
        }
        /*public ICommand GetSerieCommand
        {
            get
            {
                return new Command(async () =>
                {
                    using (var client = new HttpClient())
                    {
                        // send a GET request  
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
                        var uri = "http://phoneofficine.it/niini-gim/series";
                        var result = await client.GetStringAsync(uri);
                        //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + result);
                        //handling the answer  
                        var SerieList = JsonConvert.DeserializeObject<List<Serie>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                        Series = new ObservableCollection<Serie>(SerieList);
                    }
                });
            }
        }*/

        ObservableCollection<Serie> _series;
        public ObservableCollection<Serie> Series
        {
            get
            {
                return _series;
            }
            set
            {
                _series = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
