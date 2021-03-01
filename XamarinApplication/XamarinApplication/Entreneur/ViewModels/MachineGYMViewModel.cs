using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace XamarinApplication.Entreneur.ViewModels
{
    public class MachineGYMViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        #endregion

        #region Attributes
        private ObservableCollection<Machine> machines;
        private bool isRefreshing;
        bool _isVisibleStatus;
        private bool _showHide = false;
        private string filter;
        private List<Machine> machinesList;
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
        public ObservableCollection<Machine> Machines
        {
            get { return machines; }
            set
            {
                machines = value;
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
                isRefreshing = value;
                OnPropertyChanged();
            }
        }
        public string Filter
        {
            get { return filter; }
            set
            {
                filter = value;
                OnPropertyChanged();
                Search();
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
        public bool ShowHide
        {
            get => _showHide;
            set
            {
                _showHide = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructors
        public MachineGYMViewModel()
        {
            apiService = new ApiServices();
           
            LoadMachine();
        }
        #endregion
        #region Methods
        public async void LoadMachine()
        {
            //Current User
            var accesstoken = Settings.AccessToken;
            var username = Settings.Username;
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            var uri = "http://phoneofficine.it/niini-gim/user/username?username=" + username;
            var result = client.GetStringAsync(uri);
            User = JsonConvert.DeserializeObject<User>(result.Result);
            //Check Internet connection
            var connection = await apiService.CheckConnection();
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

            var _searchGym = new SearchByGym
            {
                gim = User.gim
            };

            var response = await apiService.GymMachine<Machine>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/machine/searchPagination?sortedBy=code&order=asc&maxResult=100",
                 accesstoken,
                 _searchGym);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            machinesList = (List<Machine>)response.Result;
            Machines = new ObservableCollection<Machine>(machinesList);
            IsRefreshing = false;
            if (Machines.Count() == 0)
            {
                IsVisibleStatus = true;
            }
            else
            {
                IsVisibleStatus = false;
            }
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadMachine);
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                ShowHide = false;
                return new RelayCommand(Search);
            }
        }

        private void Search()
        {
            if (string.IsNullOrEmpty(Filter))
            {
                Machines = new ObservableCollection<Machine>(machinesList);
                IsVisibleStatus = false;
            }
            else
            {
                Machines = new ObservableCollection<Machine>(
                    machinesList.Where(
                        l => l.code.ToLower().Contains(Filter.ToLower()) ||
                             l.serialNumber.ToLower().Contains(Filter.ToLower())));
                if (Machines.Count() == 0)
                {
                    IsVisibleStatus = true;
                }
                else
                {
                    IsVisibleStatus = false;
                }
            }
        }
        public ICommand OpenSearchBar
        {
            get
            {
                return new Command(() =>
                {
                    if (ShowHide == false)
                    {
                        ShowHide = true;
                    }
                    else
                    {
                        ShowHide = false;
                    }
                });
            }
        }
        #endregion
    }
}
