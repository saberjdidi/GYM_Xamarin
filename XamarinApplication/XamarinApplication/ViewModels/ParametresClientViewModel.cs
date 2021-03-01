using GalaSoft.MvvmLight.Command;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.Services;
using XamarinApplication.Views;

namespace XamarinApplication.ViewModels
{
    public class ParametresClientViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        private DialogService dialogService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private User users;
        private ObservableCollection<ParametreSportif> parametres;
        private List<ParametreSportif> parametresList;
        private bool isRefreshing;
        bool _isVisibleStatus;
        #endregion

        #region Constructors
        public ParametresClientViewModel()
        {
            dialogService = new DialogService();
            instance = this;
            GetParametres();
        }
        #endregion

        #region Properties
        public User User
        {
            get { return users; }
            set
            {
                users = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ParametreSportif> Parametres
        {
            get { return parametres; }
            set { SetValue(ref this.parametres, value); }
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

        #region Sigleton
        static ParametresClientViewModel instance;
        public static ParametresClientViewModel GetInstance()
        {
            if (instance == null)
            {
                return new ParametresClientViewModel();
            }

            return instance;
        }

        public void Update(ParametreSportif parametre)
        {
            IsRefreshing = true;
            var oldparametre = parametresList
                .Where(p => p.id == parametre.id)
                .FirstOrDefault();
            oldparametre = parametre;
            Parametres = new ObservableCollection<ParametreSportif>(parametresList);
            IsRefreshing = false;
        }
        public async Task Delete(ParametreSportif parametre)
        {
            IsRefreshing = true;
            var connection = await apiService.CheckConnection();
            var accesstoken = Settings.AccessToken;

            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var response = await apiService.Delete<ParametreSportif>(
                "http://phoneofficine.it",
                 "/niini-gim",
                 "/circumference",
                parametre.id,
                accesstoken);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            parametresList.Remove(parametre);
            Parametres = new ObservableCollection<ParametreSportif>(parametresList);

            IsRefreshing = false;
        }
        #endregion

        #region Methods
        public async void GetParametres()
        {
            IsRefreshing = true;
            var connection = await apiService.CheckConnection();
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
            var _parametre = new ParametreClient
            {
              sportif = User
            };

            var response = await apiService.ParametresData<ParametreSportif>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/circumference/search?sortedBy=date&order=asc&maxResult=100",
                 accesstoken,
                 _parametre);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            parametresList = (List<ParametreSportif>)response.Result;
            Parametres = new ObservableCollection<ParametreSportif>(parametresList);
            IsRefreshing = false;
            if (Parametres.Count() == 0)
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
                return new RelayCommand(GetParametres);
            }
        }
        public Command AddParametreClient
        {
            get
            {
                return new Command(() =>
                {
                   PopupNavigation.Instance.PushAsync(new AddParametreClientPage(User));
                });
            }
        }
        #endregion
    }
}
