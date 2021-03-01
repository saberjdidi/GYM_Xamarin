using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
    public class ClientViewModel : BaseViewModel
    {
        #region Services
        private RestService restService;
        private ApiServices apiService;
        private DialogService dialogService;
        #endregion

        #region Attributes
        private ObservableCollection<User> clients;
        private string filter;
        private bool isRefreshing;
        bool _isVisibleStatus;
        private bool _showHide = false;
        private List<User> clientsList;
        #endregion

        #region Constructors
        public ClientViewModel()
        {
            restService = new RestService();
            apiService = new ApiServices();
            dialogService = new DialogService();
            instance = this;
            GetClients();
        }
        #endregion

        #region Properties
        public User CurrentUser { get; set; }
        public ObservableCollection<User> Clients
        {
            get { return clients; }
            set { SetValue(ref this.clients, value); }
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

        #region Sigleton
        static ClientViewModel instance;
        public static ClientViewModel GetInstance()
        {
            if (instance == null)
            {
                return new ClientViewModel();
            }

            return instance;
        }

        public void Update(User user)
        {
            IsRefreshing = true;
            var olduser = clientsList
                .Where(p => p.id == user.id)
                .FirstOrDefault();
            olduser = user;
            Clients = new ObservableCollection<User>(clientsList);
            IsRefreshing = false;
        }
        public async Task Delete(User user)
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

            var response = await apiService.Delete<User>(
                "http://phoneofficine.it",
                 "/niini-gim",
                 "/user",
                user.id,
                accesstoken);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            clientsList.Remove(user);
            Clients = new ObservableCollection<User>(clientsList);

            IsRefreshing = false;
        }
        #endregion

        #region Methods
        public async void GetClients()
        {
            //Current User
            var accesstoken = Settings.AccessToken;
            var username = Settings.Username;
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            var uri = "http://phoneofficine.it/niini-gim/user/username?username=" + username;
            var result = client.GetStringAsync(uri);
            CurrentUser = JsonConvert.DeserializeObject<User>(result.Result);
            //Connection internet
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
            if (CurrentUser.role.name.Equals("ROLE_ADMIN"))
            {
                var _role = new Role()
                  {
                      id = 1,
                      name = "ROLE_ADMIN"
                  };

                var _roles = new List<Role>()
                      {
                          new Role(){id =  6, name= "ROLE_SPORTIF"}
                      };
                var _searchSportif = new SearchRequestByUser
                {
                    id = 1,
                    role = _role,
                    gim = null,
                    roles = _roles
                };

                var response = await restService.PostSportif<User>(
                     "http://phoneofficine.it",
                     "/niini-gim",
                     "/user/searchPagination?sortedBy=username&order=asc&maxResult=100",
                     accesstoken,
                     _searchSportif);
                if (!response.IsSuccess)
                {
                    IsRefreshing = false;
                    await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                    return;
                }
                clientsList = (List<User>)response.Result;
            }
            if (CurrentUser.role.name.Equals("ROLE_ENTRENEUR"))
            {
                var _roles = new List<Role>()
                      {
                          new Role(){id =  6, name= "ROLE_SPORTIF"}
                      };
                var _searchSportifByCoach = new SearchRequestByCoach
                {
                    id = 3,
                    coach = CurrentUser,
                    roles = _roles
                };

                var response = await restService.GetSportifOfCoach<User>(
                     "http://phoneofficine.it",
                     "/niini-gim",
                     "/user/searchPagination?sortedBy=username&order=asc&maxResult=100",
                     accesstoken,
                     _searchSportifByCoach);
                if (!response.IsSuccess)
                {
                    IsRefreshing = false;
                    await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                    return;
                }
                clientsList = (List<User>)response.Result;
            }
            Clients = new ObservableCollection<User>(clientsList);
            if (Clients.Count() == 0)
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
                return new RelayCommand(GetClients);
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
                Clients = new ObservableCollection<User>(clientsList);
                IsVisibleStatus = false;
            }
            else
            {
                Clients = new ObservableCollection<User>(
                    clientsList.Where(
                        l => l.username.ToLower().StartsWith(Filter.ToLower())));

                if (Clients.Count() == 0)
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
