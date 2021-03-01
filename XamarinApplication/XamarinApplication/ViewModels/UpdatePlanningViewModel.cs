using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
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
    public class UpdatePlanningViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        private RestService restService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private Planning _planning;
        #endregion

        #region Constructors
        public UpdatePlanningViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
            restService = new RestService();
            SelectedProgram = new List<Program>();

            ListProgramAutoComplete();
            ListUserAutoComplete();
        }
        #endregion

        #region Properties
        public Planning Planning
        {
            get { return _planning; }
            set
            {
                _planning = value;
                OnPropertyChanged();
            }
        }
        private object selectedProgram;
        public object SelectedProgram
        {
            get { return selectedProgram; }
            set
            {
                selectedProgram = value;
                // RaisePropertyChanged("SelectedItem");
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
        #endregion

        #region Methods
        public async void EditPlanning()
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
            if (Planning.sportif == null)
            {
                Value = true;
                return;
            }

            var planning = new Planning
            {
                id = Planning.id,
                date = Planning.date,
                sportif = Planning.sportif,
                programs = Planning.programs
            };
            await apiService.PutRequest<Planning>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/planning",
                 accesstoken,
                  planning);
            /* if (!response.IsSuccess)
             {
                 await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                 return;
             }*/

            Value = false;
            PlanningViewModel.GetInstance().Update(planning);

            DependencyService.Get<INotification>().CreateNotification("GYM TECH", "Planning Updated");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
        #endregion

        #region Commands
        public ICommand UpdatePlanning
        {
            get
            {
                return new Command(() =>
                {
                    EditPlanning();
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
        //**************Program**************
        private List<Program> _programAutoComplete;
        public List<Program> ProgramAutoComplete
        {
            get { return _programAutoComplete; }
            set
            {
                _programAutoComplete = value;
                OnPropertyChanged("ProgramAutoComplete");

            }
        }
        public async Task<List<Program>> ListProgramAutoComplete()
        {
            var accesstoken = Settings.AccessToken;

            var response = await apiService.GetList<Program>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/program",
                 accesstoken);
            ProgramAutoComplete = (List<Program>)response.Result;
            return ProgramAutoComplete;
        }
        //User
        public User CurrentUser { get; set; }
        private List<User> _userAutoComplete;
        public List<User> UserAutoComplete
        {
            get { return _userAutoComplete; }
            set
            {
                _userAutoComplete = value;
                OnPropertyChanged();
            }
        }
        public async Task<List<User>> ListUserAutoComplete()
        {
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
                UserAutoComplete = (List<User>)response.Result;
            }
            if (CurrentUser.role.name.Equals("ROLE_ENTRENEUR"))
            {
                var _roles = new List<Role>()
                  {
                      new Role(){id =  6, name= "ROLE_SPORTIF"}
                  };
                var _search = new SearchUserOfPlanning
                {
                    coach = CurrentUser,
                    roles = _roles
                };

                var response = await apiService.SearchUserOfPlanning<User>(
                     "http://phoneofficine.it",
                     "/niini-gim",
                     "/user/searchPagination?sortedBy=firstname&order=asc&maxResult=100",
                     accesstoken,
                     _search);
                UserAutoComplete = (List<User>)response.Result;
            }
            return UserAutoComplete;
        }
        #endregion
    }
}
