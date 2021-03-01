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
    public class GimViewModel : BaseViewModel
    {
        #region Services
        private RestService restService;
        private ApiServices apiService;
        private DialogService dialogService;
        #endregion

        #region Attributes
        private ObservableCollection<Gim> gims;
        private bool isRefreshing;
        bool _isVisibleStatus;
        private bool _showHide = false;
        private string filter;
        private List<Gim> gimsList;
        #endregion
     
        #region Properties
        public ObservableCollection<Gim> Gims
        {
            get { return gims; }
            set {
                gims = value;
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
        public GimViewModel()
        {
            restService = new RestService();
            apiService = new ApiServices();
            dialogService = new DialogService();
            instance = this;
            LoadGim();
        }
        #endregion

        #region Sigleton
        static GimViewModel instance;
        public static GimViewModel GetInstance()
        {
            if (instance == null)
            {
                return new GimViewModel();
            }

            return instance;
        }

        public void Update(Gim gim)
        {
            IsRefreshing = true;
            var oldgim = gimsList
                .Where(p => p.Id == gim.Id)
                .FirstOrDefault();
            oldgim = gim;
            Gims = new ObservableCollection<Gim>(gimsList);
            IsRefreshing = false;
        }
        public async Task Delete(Gim gim)
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

            var response = await apiService.Delete<Gim>(
                "http://phoneofficine.it",
                 "/niini-gim",
                 "/gim",
                gim.Id,
                accesstoken);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            gimsList.Remove(gim);
            Gims = new ObservableCollection<Gim>(gimsList);

            IsRefreshing = false;
        }
        #endregion

        #region Methods
        public async void LoadGim()
         {
           IsRefreshing = true;
            var connection = await restService.CheckConnection();
            var accesstoken = Settings.AccessToken;

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

            var response = await restService.GetList<Gim>(
                 "http://phoneofficine.it", 
                 "/niini-gim", 
                 "/gim", 
                 accesstoken);
            if (!response.IsSuccess)
            {
               IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            gimsList = (List<Gim>)response.Result;
            Gims = new ObservableCollection<Gim>(gimsList);
            IsRefreshing = false;
         }
        /*public async void LoadGim()
        {
            using (var client = new HttpClient())
            {
                // send a GET request  
                var accesstoken = Settings.AccessToken;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
                var uri = "http://phoneofficine.it/niini-gim/gim";
                var result = await client.GetStringAsync(uri);
                //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + result);
                //handling the answer  
                var GimList = JsonConvert.DeserializeObject<List<Gim>>(result);

                Gims = new ObservableCollection<Gim>(GimList);

            }
        }*/
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadGim);
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
                Gims = new ObservableCollection<Gim>(gimsList);
                IsVisibleStatus = false;
            }
            else
            {
                Gims = new ObservableCollection<Gim>(
                    gimsList.Where(
                        l => l.Code.ToLower().StartsWith(Filter.ToLower()) ||
                             l.Name.ToLower().StartsWith(Filter.ToLower())));

                if (Gims.Count() == 0)
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
