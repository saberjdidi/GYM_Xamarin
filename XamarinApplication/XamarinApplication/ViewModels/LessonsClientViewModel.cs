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
    public class LessonsClientViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        private DialogService dialogService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private User users;
        private ObservableCollection<Client> clients;
        private List<Client> clientList;
        private bool isRefreshing;
        bool _isVisibleStatus;
        #endregion

        #region Constructors
        public LessonsClientViewModel()
        {
            dialogService = new DialogService();
            instance = this;
            GetClients();
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
        public ObservableCollection<Client> Clients
        {
            get { return clients; }
            set { SetValue(ref this.clients, value); }
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
        static LessonsClientViewModel instance;
        public static LessonsClientViewModel GetInstance()
        {
            if (instance == null)
            {
                return new LessonsClientViewModel();
            }

            return instance;
        }

        public async Task Delete(Client parametre)
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

            var response = await apiService.Delete<Client>(
                "http://phoneofficine.it",
                 "/niini-gim",
                 "/lesson",
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

            clientList.Remove(parametre);
            Clients = new ObservableCollection<Client>(clientList);

            IsRefreshing = false;
        }
        #endregion

        #region Methods
        public async void GetClients()
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
            var _searchLesson = new SearchLesson
            {
                customerCard = User
            };

            var response = await apiService.LessonsData<Client>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/lesson/search?sortedBy=number&order=asc&maxResult=100",
                 accesstoken,
                 _searchLesson);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            clientList = (List<Client>)response.Result;
            Clients = new ObservableCollection<Client>(clientList);
            IsRefreshing = false;
            if (Clients.Count() == 0)
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
                return new RelayCommand(GetClients);
            }
        }
        public Command AddLessonClient
        {
            get
            {
                return new Command(() =>
                {
                    PopupNavigation.Instance.PushAsync(new AddLessonClientPage(User));
                });
            }
        }
        #endregion
    }
}
