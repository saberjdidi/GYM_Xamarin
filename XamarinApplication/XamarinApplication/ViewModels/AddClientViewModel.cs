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
    public class AddClientViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        private RestService restService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private bool _showHide = false;
        #endregion

        #region Constructor
        public AddClientViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
            apiService = new ApiServices();
            restService = new RestService();

            ListGYMAutoComplete();
            //ListCoachAutoComplete();
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
                }else
                {
                    Hide = false;
                }
        }
        #endregion

        #region Properties
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }
        public string Height { get; set; }
        public string Frequency { get; set; }

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
        private User _coach = null;
        public User Coach
        {
            get { return _coach; }
            set
            {
                _coach = value;
                OnPropertyChanged();
            }
        }
        private DateTime _birthdate = System.DateTime.Today;  //DateTime.Today.Date;
        public DateTime BirthDate
        {
            get { return _birthdate; }
            set
            {
                _birthdate = value;
                OnPropertyChanged();
            }
        }
        private bool _enabled = true;
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
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
        public bool ShowHide
        {
            get => _showHide;
            set
            {
                _showHide = value;
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
        public async void AddClient()
        {
            var accesstoken = Settings.AccessToken;
            //connection internet
            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Warning,
                    Languages.CheckConnection,
                    Languages.Ok);
                return;
            }
            Value = true;
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Firstname) || string.IsNullOrEmpty(Lastname))
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
                if (Gim == null || Coach == null)
                {
                    Value = true;
                    return;
                }
                var _role = new Role()
                {
                    id = 6,
                    name = "ROLE_SPORTIF" //ROLE_SYSTEM
                };
                var user = new AddUser
                {
                    username = Username,
                    password = Password,
                    firstname = Firstname,
                    lastname = Lastname,
                    email = Email,
                    role = _role,
                    gim = Gim,
                    coach = Coach,
                    sex = SelectedSex.Key,
                    bia = SelectedBia.Key,
                    phone = Phone,
                    birthdate = BirthDate,
                    height = Height,
                    frequency = Frequency,
                    note = Note,
                    enabled = Enabled
                };
                await apiService.SaveRequest<AddUser>(
                     "http://phoneofficine.it",
                     "/niini-gim",
                     "/user/create",
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
                var user = new AddUser
                {
                    username = Username,
                    password = Password,
                    firstname = Firstname,
                    lastname = Lastname,
                    email = Email,
                    role = _role,
                    gim = CurrentUser.gim,
                    coach = CurrentUser,
                    sex = SelectedSex.Key,
                    bia = SelectedBia.Key,
                    phone = Phone,
                    birthdate = BirthDate,
                    height = Height,
                    frequency = Frequency,
                    note = Note,
                    enabled = Enabled
                };
                await apiService.SaveRequest<AddUser>(
                     "http://phoneofficine.it",
                     "/niini-gim",
                     "/user/create",
                     accesstoken,
                     user);
            }
           /* if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }*/
            Value = false;
            DependencyService.Get<INotification>().CreateNotification("GYM TECH", "User Added");
            await Navigation.PopPopupAsync();
        }
        #endregion

        #region Commands
        public ICommand SaveClient
        {
            get
            {
                return new Command(() =>
                {
                    AddClient();
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
        public ICommand OpenCoachCommand
        {
            get
            {
                return new Command(() =>
                {
                    if(Gim == null)
                    {
                        Application.Current.MainPage.DisplayAlert("Alert", "Please Select GYM", "ok");
                        return;
                    }
                    else
                    {
                        ListCoachAutoComplete();
                        ShowHide = true;
                    }
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
            //method 1
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
            //method 2
         /*   var _searchCoach = new SearchByGym
            {
                gim = Gim
            };
            var response = await apiService.GymMachine<User>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/user/search?sortedBy=username&order=asc",
                 accesstoken,
                 _searchCoach);
            */
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
