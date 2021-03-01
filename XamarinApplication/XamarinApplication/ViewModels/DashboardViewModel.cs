using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.Services;
using GalaSoft.MvvmLight.Command;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Diagnostics;

namespace XamarinApplication.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        #region Services
        private RestService restService;
        private ApiServices apiService;
        #endregion

        #region Constructors
        public DashboardViewModel()
        {
            restService = new RestService();
            apiService = new ApiServices();
            Chart = new ObservableCollection<Chart>();
            ChartData();
            LoadMachine();
        }
        #endregion

        #region Attributes
        private ObservableCollection<Machine> machines;
        private ObservableCollection<Chart> charts;
        private bool isRefreshing;
        bool _isVisibleStatus;
        private bool _showHide = false;
        private string filter;
        private List<Machine> machinesList;
        private List<int> chartsList;
        #endregion

        #region Properties
        public ObservableCollection<Chart> Chart
        {
            get { return charts; }
            set
            {
                charts = value;
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
        public User User { get; set; }
        #endregion

        #region Methods
        public async void ChartData()
        {

            using (var client = new HttpClient())
            {
                var accesstoken = Settings.AccessToken;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
                var uri = "http://phoneofficine.it/niini-gim/machine/getCountMachine";
                var result = await client.GetStringAsync(uri);
                chartsList = JsonConvert.DeserializeObject<List<int>>(result);

                Chart chart1 = new Chart()
                    {
                        name = "In Ligne",
                        quantity = chartsList[0]
                    };
                Chart.Add(chart1);
                Chart chart2 = new Chart()
                    {
                        name = "Off Ligne",
                        quantity = chartsList[1]
                    };
                Chart.Add(chart2);
                Chart chart3 = new Chart()
                    {
                        name = "In Stand",
                        quantity = chartsList[2]
                    };
                Chart.Add(chart3);
               
            }
        }

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
            IsRefreshing = true;
            if (User.role.name.Equals("ROLE_ADMIN"))
            {
                var response = await restService.GetList<Machine>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/machine",
                 accesstoken);
                if (!response.IsSuccess)
                {
                    IsRefreshing = false;
                    await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                    return;
                }
               machinesList = (List<Machine>)response.Result;
            }
            if (User.role.name.Equals("ROLE_ENTRENEUR"))
            {
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
            }
            Machines = new ObservableCollection<Machine>(machinesList);
            IsRefreshing = false;
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
                        l => l.serialNumber.ToLower().StartsWith(Filter.ToLower()) ||
                             l.description.ToLower().StartsWith(Filter.ToLower()) ||
                             l.gim.Name.ToLower().StartsWith(Filter.ToLower())));
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
