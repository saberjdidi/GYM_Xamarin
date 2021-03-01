using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.Services;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Entreneur.ViewModels
{
    public class GYMEntreneurViewModel : BaseViewModel
    {
        #region Services
        private RestService restService;
        #endregion
        #region Attributes
        private bool isRefreshing;
        bool _isVisibleStatus;
        private List<User> PlanningList;
        ObservableCollection<User> _gim;
        #endregion

        #region Constructors
        public GYMEntreneurViewModel()
        {
            restService = new RestService();
            ListGYM();
        }
        #endregion

        #region Properties
        private User _user = null;
        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<User> Gims
        {
            get
            {
                return _gim;
            }
            set
            {
                _gim = value;
                OnPropertyChanged();
            }
        }
        public bool IsRefreshing
        {
            get
            {
                return isRefreshing;
            }
            set
            {
                SetValue(ref this.isRefreshing, value);
            }
        }
        #endregion

        #region Methods
        public async void ListGYM()
        {
            //Connection Internet
            var connection = await restService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Ok");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }
            //Current User
            IsRefreshing = true;
            var accesstoken = Settings.AccessToken;
            var username = Settings.Username;
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            var uri = "http://phoneofficine.it/niini-gim/user/username?username=" + username;
            var result = client.GetStringAsync(uri);
            User = JsonConvert.DeserializeObject<User>(result.Result);
            Debug.WriteLine("********User********");
            Debug.WriteLine(User);
            //handling the answer  
            // PlanningList = JsonConvert.DeserializeObject<List<User>>(result.Result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            // Gims = new ObservableCollection<User>(PlanningList);
            IsRefreshing = false;

        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(ListGYM);
            }
        }
        #endregion
    }
}
