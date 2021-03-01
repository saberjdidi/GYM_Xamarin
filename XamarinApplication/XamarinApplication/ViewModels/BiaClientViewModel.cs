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
    public class BiaClientViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        private DialogService dialogService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private User users;
        private ObservableCollection<BiaSportif> _bia;
        private List<BiaSportif> biaList;
        private bool isRefreshing;
        bool _isVisibleStatus;
        #endregion

        #region Constructors
        public BiaClientViewModel()
        {
            dialogService = new DialogService();
            instance = this;
            GetBia();
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
        public ObservableCollection<BiaSportif> Bia
        {
            get { return _bia; }
            set { SetValue(ref this._bia, value); }
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
        static BiaClientViewModel instance;
        public static BiaClientViewModel GetInstance()
        {
            if (instance == null)
            {
                return new BiaClientViewModel();
            }

            return instance;
        }
        public void Update(BiaSportif bia)
        {
            IsRefreshing = true;
            var oldbia = biaList
                .Where(p => p.id == bia.id)
                .FirstOrDefault();
            oldbia = bia;
            Bia = new ObservableCollection<BiaSportif>(biaList);
            IsRefreshing = false;
        }
        public async Task Delete(BiaSportif bia)
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

            var response = await apiService.Delete<BiaSportif>(
                "http://phoneofficine.it",
                 "/niini-gim",
                 "/bia",
                bia.id,
                accesstoken);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            biaList.Remove(bia);
            Bia = new ObservableCollection<BiaSportif>(biaList);

            IsRefreshing = false;
        }
        #endregion

        #region Methods
        public async void GetBia()
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

            var response = await apiService.ParametresData<BiaSportif>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/bia/search?sortedBy=date&order=asc&maxResult=100",
                 accesstoken,
                 _parametre);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            biaList = (List<BiaSportif>)response.Result;
            Bia = new ObservableCollection<BiaSportif>(biaList);
            IsRefreshing = false;
            if (Bia.Count() == 0)
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
                return new RelayCommand(GetBia);
            }
        }
        public Command AddBiaClient
        {
            get
            {
                return new Command(() =>
                {
                    PopupNavigation.Instance.PushAsync(new AddBiaClientPage(User));
                });
            }
        }
        #endregion
    }
}
