using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
    public class PlanningViewModel : BaseViewModel
    {
        #region Services
        private RestService restService = new RestService();
        private ApiServices apiService;
        private DialogService dialogService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private bool isRefreshing;
        bool _isVisibleStatus;
        private bool _showHide = false;
        private string filter;
        private List<Planning> PlanningList;
        ObservableCollection<Planning> _plannings;
        #endregion

        #region Constructors
        public PlanningViewModel()
        {
            apiService = new ApiServices();
            dialogService = new DialogService();
            instance = this;
        }
        #endregion

        #region Sigleton
        static PlanningViewModel instance;
        public static PlanningViewModel GetInstance()
        {
            if (instance == null)
            {
                return new PlanningViewModel();
            }

            return instance;
        }

        public void Update(Planning planning)
        {
            IsRefreshing = true;
            var oldplanning = PlanningList
                .Where(p => p.id == planning.id)
                .FirstOrDefault();
            oldplanning = planning;
            Plannings = new ObservableCollection<Planning>(PlanningList);
            IsRefreshing = false;
        }
        public async Task Delete(Planning planning)
        {
            IsRefreshing = true;
            var connection = await restService.CheckConnection();
            var accesstoken = Settings.AccessToken;

            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var response = await apiService.Delete<Planning>(
                "http://phoneofficine.it",
                 "/niini-gim",
                 "/planning",
                planning.id,
                accesstoken);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            PlanningList.Remove(planning);
            Plannings = new ObservableCollection<Planning>(PlanningList);

            IsRefreshing = false;
        }
        #endregion

        #region Methods
        public async void GetPlannings()
        {
            using (var client = new HttpClient())
            {
                // send a GET request with authorization 
                var accesstoken = Settings.AccessToken;
                var connection = await restService.CheckConnection();
                IsRefreshing = true;

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

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
                var uri = "http://phoneofficine.it/niini-gim/planning";
                var result = await client.GetStringAsync(uri);
                //handling the answer  
                PlanningList = JsonConvert.DeserializeObject<List<Planning>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                Plannings = new ObservableCollection<Planning>(PlanningList);
                IsRefreshing = false;
            }
        }
        #endregion

        #region Properties
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

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(GetPlannings);
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
                Plannings = new ObservableCollection<Planning>(PlanningList);
                IsVisibleStatus = false;
            }
            else
            {
                Plannings = new ObservableCollection<Planning>(
                    PlanningList.Where(
                        l => l.sportif.username.ToLower().Contains(Filter.ToLower()) ||
                             l.sportif.lastname.ToLower().Contains(Filter.ToLower())));

                if (Plannings.Count() == 0)
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
