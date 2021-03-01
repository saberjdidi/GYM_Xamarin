using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.Services;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Sportif.ViewModels
{
    public class PlanningSportifViewModel : BaseViewModel
    {
        #region Services
        private RestService restService;
        private ApiServices apiService;
        #endregion

        #region Attributes
        private bool isRefreshing;
        bool _isVisibleStatus;
        private List<Planning> PlanningList;
        ObservableCollection<Planning> _plannings;
        #endregion

        #region Constructors
        public PlanningSportifViewModel()
        {
            restService = new RestService();
            apiService = new ApiServices();
            GetPlannings();
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
        public ObservableCollection<Planning> Plannings
        {
            get
            {
                return _plannings;
            }
            set
            {
                _plannings = value;
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
        public bool IsVisibleStatus
        {
            get { return _isVisibleStatus; }
            set
            {
                _isVisibleStatus = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Methods
        public async void GetPlannings()
        {
            //Current User
            var accesstoken = Settings.AccessToken;
            var username = Settings.Username;
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            var uri = "http://phoneofficine.it/niini-gim/user/username?username=" + username;
            var result = client.GetStringAsync(uri);
            User = JsonConvert.DeserializeObject<User>(result.Result);
            Debug.WriteLine("********User Menu********");
            Debug.WriteLine(User);
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
            //Planning method
            IsRefreshing = true;
            var _sportif = new ParametreClient
            {
                sportif = User
            };

            var response = await apiService.ParametresData<Planning>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/planning/searchPagination?sortedBy=date&order=asc&maxResult=100",
                 accesstoken,
                 _sportif);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            PlanningList = (List<Planning>)response.Result;
            Plannings = new ObservableCollection<Planning>(PlanningList);
            if (Plannings.Count() == 0)
            {
                IsVisibleStatus = true;
            }
            IsRefreshing = false;
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(GetPlannings);
            }
        }
        #endregion
    }
}
