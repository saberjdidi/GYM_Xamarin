using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
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
    public class UpdateClientViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        private RestService restService = new RestService();
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private User user;
        #endregion

        #region Constructors
        public UpdateClientViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
            ListGYMAutoComplete();
            ListCoachAutoComplete();
            ListSex = GetSex().OrderBy(t => t.Value).ToList();
            ListBia = GetBia().OrderBy(t => t.Value).ToList();

            //Current User
            var accesstoken = Settings.AccessToken;
            var username = Settings.Username;
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            var uri = "http://phoneofficine.it/niini-gim/user/username?username=" + username;
            var result = client.GetStringAsync(uri);
            CurrentUser = JsonConvert.DeserializeObject<User>(result.Result);
            if (CurrentUser.role.name.Equals("ROLE_ADMIN"))
            {
                Hide = true;
            }
            else
            {
                Hide = false;
            }
        }
        #endregion

        #region Properties
        public User User
        {
            get { return user; }
            set
            {
                user = value;
                OnPropertyChanged();
            }
        }
        private Gim gim;
        public Gim Gim
        {
            get { return gim; }
            set
            {
                gim = value;
                OnPropertyChanged();
            }
        }
        private bool value = false;
        public bool Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnPropertyChanged();
            }
        }
        public User CurrentUser { get; set; }
        private bool _hide;
        public bool Hide
        {
            get => _hide;
            set
            {
                _hide = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Methods
        public async void EditClient()
        {
            Value = true;
            var connection = await apiService.CheckConnection();
            var accesstoken = Settings.AccessToken;
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Warning,
                    Languages.CheckConnection,
                    Languages.Ok);
                return;
            }
            if (string.IsNullOrEmpty(User.username) || string.IsNullOrEmpty(User.password) || string.IsNullOrEmpty(User.firstname) || string.IsNullOrEmpty(User.lastname))
            {
                Value = true;
                return;
            }
            if (SelectedSex == null)
            {
                Value = true;
                await Application.Current.MainPage.DisplayAlert("Alert", "Selected Sex", "ok");
                return;
            }
            if (SelectedBia == null)
            {
                Value = true;
                await Application.Current.MainPage.DisplayAlert("Alert", "Selected Bia", "ok");
                return;
            }
            if (CurrentUser.role.name.Equals("ROLE_ADMIN"))
            {
                if (User.gim == null || User.coach == null)
                {
                    Value = true;
                    return;
                }
                var _role = new Role()
                {
                    id = 6,
                    name = "ROLE_SPORTIF"
                };

                var user = new User
                {
                    id = User.id,
                    role = _role,
                    username = User.username,
                    password = User.password,
                    firstname = User.firstname,
                    lastname = User.lastname,
                    email = User.email,
                    gim = User.gim,
                    coach = User.coach,
                    phone = User.phone,
                    birthdate = User.birthdate,
                    sex = SelectedSex.Key,
                    bia = SelectedBia.Key,
                    height = User.height,
                    frequency = User.frequency,
                    note = User.note,
                    enabled = User.enabled

                };
                await apiService.PutRequest<User>(
                     "http://phoneofficine.it",
                     "/niini-gim",
                     "/user",
                     accesstoken,
                      user);
            }
            if (CurrentUser.role.name.Equals("ROLE_ENTRENEUR"))
            {
                var _role = new Role()
                {
                    id = 6,
                    name = "ROLE_SPORTIF"
                };

                var user = new User
                {
                    id = User.id,
                    role = _role,
                    username = User.username,
                    password = User.password,
                    firstname = User.firstname,
                    lastname = User.lastname,
                    email = User.email,
                    gim = CurrentUser.gim,
                    coach = CurrentUser,
                    phone = User.phone,
                    birthdate = User.birthdate,
                    sex = SelectedSex.Key,
                    bia = SelectedBia.Key,
                    height = User.height,
                    frequency = User.frequency,
                    note = User.note,
                    enabled = User.enabled

                };
                await apiService.PutRequest<User>(
                     "http://phoneofficine.it",
                     "/niini-gim",
                     "/user",
                     accesstoken,
                      user);
            }
            Value = false;
            ClientViewModel.GetInstance().Update(user);

            DependencyService.Get<INotification>().CreateNotification("GYM TECH", "User Updated");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
        #endregion

        #region Commands
        public ICommand UpdateClient
        {
            get
            {
                return new Command(() =>
                {
                    EditClient();
                });
            }
        }
        public Command ClosePopup
        {
            get
            {
                return new Command(() =>
                {
                    Navigation.PopPopupAsync();
                });
            }
        }
        #endregion

        #region Autocomplete
        //**************GYM**************
        private List<Gim> _gymAutoComplete;
        public List<Gim> GYMAutoComplete
        {
            get { return _gymAutoComplete; }
            set
            {
                _gymAutoComplete = value;
                OnPropertyChanged();
            }
        }
        public async Task<List<Gim>> ListGYMAutoComplete()
        {
            var accesstoken = Settings.AccessToken;

            var response = await apiService.GetList<Gim>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/gim",
                 accesstoken);
            GYMAutoComplete = (List<Gim>)response.Result;
            return GYMAutoComplete;
        }
        //**********Coach***********
        private List<User> _coachAutoComplete;
        public List<User> CoachAutoComplete
        {
            get { return _coachAutoComplete; }
            set
            {
                _coachAutoComplete = value;
                OnPropertyChanged();
            }
        }
        public async Task<List<User>> ListCoachAutoComplete()
        {
            var accesstoken = Settings.AccessToken;
            var _roles = new List<Role>()
            {
                      new Role(){id =  2, name= "ROLE_ENTRENEUR"}
            };
            var _searchCoach = new SearchCoachByGym
            {
                gim = Gim,
                roles = _roles
            };
            var response = await restService.SearchCoachByGym<User>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/user/searchPagination?sortedBy=username&order=asc&maxResult=100",
                 accesstoken,
                 _searchCoach);

            CoachAutoComplete = (List<User>)response.Result;
            return CoachAutoComplete;
        }
        #endregion

        #region Sex & Bia
        //***********Sex**********
        public List<Language> ListSex { get; set; }
        private Language _selectedSex { get; set; }
        public Language SelectedSex
        {
            get { return _selectedSex; }
            set
            {
                if (_selectedSex != value)
                {
                    _selectedSex = value;
                    OnPropertyChanged();
                }
            }
        }
        public List<Language> GetSex()
        {
            var languages = new List<Language>()
            {
                new Language(){Key =  "M", Value= "Male"},
                new Language(){Key =  "F", Value= "Female"}
            };

            return languages;
        }
        //***********Bia**************
        public List<Language> ListBia { get; set; }
        private Language _selectedBia { get; set; }
        public Language SelectedBia
        {
            get { return _selectedBia; }
            set
            {
                if (_selectedBia != value)
                {
                    _selectedBia = value;
                    OnPropertyChanged();
                }
            }
        }
        public List<Language> GetBia()
        {
            var languages = new List<Language>()
            {
                new Language(){Key =  "si", Value= "Si"},
                new Language(){Key =  "no", Value= "No"}
            };

            return languages;
        }
        #endregion
    }
}
