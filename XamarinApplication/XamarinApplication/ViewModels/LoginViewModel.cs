using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Interfaces;
using XamarinApplication.Resources;
using XamarinApplication.Services;
using XamarinApplication.Views;

namespace XamarinApplication.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private ApiServices _apiServices = new ApiServices();
        public INavigation Navigation { get; set; }
        public LoginViewModel(INavigation _navigation)
        {
            Navigation = _navigation;

        }

        public string Username { get; set; }
        public string Password { get; set; }

        private bool value = false;
        public bool Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand
        {
            get
            {
                return new Command(async () =>
                {
                    Value = true;
                    var client = new HttpClient();
                    var connection = await _apiServices.CheckConnection();

                    if (!connection.IsSuccess)
                    {
                        await Application.Current.MainPage.DisplayAlert(
                            Languages.Warning,
                            Languages.CheckConnection,
                            Languages.Ok);
                        return;
                    }
                    if (string.IsNullOrEmpty(Username))
                    {
                        Value = true;
                        //await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.UsernameValidation, Languages.Ok);
                        return;
                    }
                    if (string.IsNullOrEmpty(Password))
                    {
                        Value = true;
                        //await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.PasswordValidation, Languages.Ok);
                        return;
                    }

                    var json = JsonConvert.SerializeObject(new { username = Username, password = Password });
                    HttpContent content = new StringContent(json);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response = await client.PostAsync("http://phoneofficine.it/niini-gim/login", content);

                    var token = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("-------------------");
                    Debug.WriteLine(token);
                    Debug.WriteLine(response);

                    if (response.IsSuccessStatusCode)
                    {
                        Value = false;
                        var jwt = response.Headers.GetValues("Authorization").FirstOrDefault();
                        Debug.WriteLine("********");
                        Debug.WriteLine(jwt);
                        //storage of token
                        Settings.AccessToken = jwt;
                        Settings.Username = Username;

                        MainViewModel.GetInstance().Programs = new ProgramViewModel();
                        //MainViewModel.GetInstance().Exercices = new ExerciceViewModel();
                        //MainViewModel.GetInstance().Machines = new MachineViewModel();
                        //await Application.Current.MainPage.DisplayAlert("Message", "Login successfully", "ok");
                        //await ProgressBar.ProgressProperty(0.8, 900, Easing.Linear);

                        PopuPage page1 = new PopuPage();
                        await PopupNavigation.Instance.PushAsync(page1);
                        await Task.Delay(2000);
                        MessagingCenter.Send<LoginViewModel>(this, "Hi");

                        await Navigation.PushModalAsync(new MainPage());
                        
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.FailedLogin, Languages.Ok);
                    }

                    
                });
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        public LoginViewModel()
        {
            Username = Settings.Username;
            Password = Settings.Password;
        }
        /*public ICommand LoginCommand
        {
            get
            {
                return new Command(async() =>
                {
                   var accesstoken = await _apiServices.LoginAsync(Username, Password);

                    Settings.AccessToken = accesstoken;
                    if(Settings.AccessToken == accesstoken)
                    {
                        await Application.Current.MainPage.DisplayAlert("Message", "Login successfully", "Ok");
                        await Navigation.PushModalAsync(new MainPage());
                    }
                    else if(Settings.AccessToken != accesstoken)
                    {
                        await Application.Current.MainPage.DisplayAlert("Message", "Failed to Login", "Ok");
                        //await Navigation.PushModalAsync(new LoginPage());
                    }
                });
            }
        }*/

        /*public ICommand LoginCommand
       {
           get
           {
               return new Command(async () =>
               {
                   var accesstoken = await _apiServices.LoginGym(Username, Password);
                   await Navigation.PushModalAsync(new MainPage());
               });
           }
       }*/


        // MultiLanguages
       /* public List<Language> CitiesList { get; set; }

        private Language _selectedCity { get; set; }
        public Language SelectedCity
        {
            get { return _selectedCity; }
            set
            {
                if (_selectedCity != value)
                {
                    _selectedCity = value;
                    Resource.Culture = new CultureInfo(_selectedCity.Key);
                }
            }
        }
        public List<Language> GetLanguage()
        {
            var languages = new List<Language>()
            {
                new Language(){Key =  "en", Value= "English"},
                new Language(){Key =  "fr", Value= "Francais"},
                new Language(){Key =  "it", Value= "Italian"}
            };

            return languages;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        */
        
    }

    
}
