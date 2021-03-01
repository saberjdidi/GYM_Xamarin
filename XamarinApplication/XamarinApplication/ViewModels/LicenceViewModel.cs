using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
    public class LicenceViewModel : BaseViewModel
    {
        #region Services
        private RestService restService;
        private ApiServices apiService;
        private DialogService dialogService;
        #endregion

        #region Attributes
        private ObservableCollection<Licence> licences;
        private string filter;
        private bool isRefreshing;
        bool _isVisibleStatus;
        private bool _showHide = false;
        private List<Licence> licencesList;
        public string AccessToken { get; set; }
        #endregion

        #region Properties
        public ObservableCollection<Licence> Licences
        {
            get { return licences; }
            set { SetValue(ref this.licences, value); }
        }
        public string Filter
        {
            get { return filter; }
            set
            {
                SetValue(ref filter, value);
                Search();
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
        public LicenceViewModel()
        {
            restService = new RestService();
            apiService = new ApiServices();
            dialogService = new DialogService();
            instance = this;
            LoadLicence();
        }
        #endregion

        #region Sigleton
        static LicenceViewModel instance;
        public static LicenceViewModel GetInstance()
        {
            if (instance == null)
            {
                return new LicenceViewModel();
            }

            return instance;
        }

        public void Update(Licence licence)
        {
            IsRefreshing = true;
            var oldlicence = licencesList
                .Where(p => p.id == licence.id)
                .FirstOrDefault();
            oldlicence = licence;
            Licences = new ObservableCollection<Licence>(licencesList);
            IsRefreshing = false;
        }
        public async Task Delete(Licence licence)
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

            var response = await apiService.Delete<Licence>(
                "http://phoneofficine.it",
                 "/niini-gim",
                 "/licence",
                licence.id,
                accesstoken);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            licencesList.Remove(licence);
            Licences = new ObservableCollection<Licence>(licencesList);

            IsRefreshing = false;
        }
        #endregion

        #region Methods
        public async void LoadLicence()
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
            var _searchRequest = new SearchRequest
            {
            };

            var response = await restService.PostData<Licence>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/licence/search?sortedBy=name&order=desc&maxResult=200",
                 //"/licence/searchPagination?sortedBy=name&order=desc&maxResult=8&firstResult=1",
                 accesstoken,
                 _searchRequest);
            if (!response.IsSuccess)
            {
              IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            licencesList = (List<Licence>)response.Result;
            Licences = new ObservableCollection<Licence>(licencesList);
          IsRefreshing = false;
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadLicence);
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
                Licences = new ObservableCollection<Licence>(licencesList);
                IsVisibleStatus = false;
            }
            else
            {
                Licences = new ObservableCollection<Licence>(
                    licencesList.Where(
                        l => l.number.ToLower().Contains(Filter.ToLower()) ||
                             l.name.ToLower().Contains(Filter.ToLower())));

                if (Licences.Count() == 0)
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
