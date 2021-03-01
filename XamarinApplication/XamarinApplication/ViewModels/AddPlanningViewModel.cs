using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
    public class AddPlanningViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        private RestService restService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        #endregion

        #region Constructor
        public AddPlanningViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
            apiService = new ApiServices();
            restService = new RestService();
            SelectedProgram = new List<Program>();
            
            ListProgramAutoComplete();
            ListUserAutoComplete();
            ListClientsAutoComplete();
        }
        #endregion

        #region Properties
        private DateTime _date = System.DateTime.Today;  //DateTime.Today.Date;
        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged();
            }
        }
        private User _user = null;
        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
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
        bool _isRunning = true;
        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
            set
            {
                this._isRunning = value;
                OnPropertyChanged();
            }
        }
        bool _isEnabled = true;
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                this._isEnabled = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Methods
        public async void AddPlanning()
        {
            Value = true;
            IsRunning = false;
            IsEnabled = false;
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
            
            if (Date == null)
            {
                IsEnabled = false;
                Value = true;
                return;
            }
            if(User == null)
            {
                IsEnabled = false;
                Value = true;
                await Application.Current.MainPage.DisplayAlert("Alert", "Athletic is Required", "ok");
                return;
            }

            var planning = new AddPlanning
            {
                date = Date,
                sportif = User,
                programs = SelectedProgram
            };
            if(planning.programs == null)
            {
                Value = true;
                return;
            }
            await apiService.SaveRequest<AddPlanning>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/planning/create",
                 accesstoken,
                 planning);

            /* if (!response.IsSuccess)
             {
                 await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                 return;
             }*/

            Value = false;
            IsRunning = true;
            IsEnabled = true;
            DependencyService.Get<INotification>().CreateNotification("GYM TECH", "Planning Added");
            await Navigation.PopPopupAsync();
        }
        #endregion

        #region Commands
        public ICommand SavePlanning
        {
            get
            {
                return new Command(() =>
                {
                    AddPlanning();
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
          private List<User> _userAutoCompleteSportif;
           public List<User> UserAutoCompleteSportif
           {
               get { return _userAutoCompleteSportif; }
               set
               {
                   _userAutoCompleteSportif = value;
                   OnPropertyChanged();
               }
          }
        public async Task<List<User>> ListUserAutoComplete()
        {

            var accesstoken = Settings.AccessToken;
                var _searchRequest = new SearchRequest
                            {
                            };
                var response = await restService.PostData<User>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/user/search?sortedBy=username&order=asc",
                 accesstoken,
                 _searchRequest);

               UserAutoComplete = (List<User>)response.Result;
                /*  UserAutoComplete.ForEach(sportif => {
                      Debug.WriteLine("+++++++++++++++++++++++++sportif++++++++++++++++++++++++");
                      Debug.WriteLine(sportif.role.name);
                      if (sportif.role.name.Equals("ROLE_SPORTIF"))
                      {
                          Debug.WriteLine("+++++++++++++++++++++++++test if++++++++++++++++++++++++");
                          Debug.WriteLine(sportif.username);
                          UserAutoCompleteSportif.Add(sportif);
                          Debug.WriteLine("+++++++++++++++++++++++++UserAutoCompleteSportif++++++++++++++++++++++++");
                          Debug.WriteLine(UserAutoCompleteSportif);
                      }
                  });*/
                return UserAutoComplete;
            
        }
        /*
        //**************Auther**************
        private List<Client> _ideas;
        public List<Client> Ideas
        {
            get { return _ideas; }
            set
            {
                _ideas = value;
                OnPropertyChanged();
            }
        }
        public async Task<List<Client>> PostIdeaAsync()
        {
            var _searchRequestByUser = new SearchRequestByUser
            {
                id = 1,
                //firstname = "",
                role = { id = 1, name = "ROLE_ADMIN" }
                //role = { name = "ROLE_SPORTIF" }
            };
            var _searchRequest = new SearchRequest
            {
            };
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.AccessToken);

            var json = JsonConvert.SerializeObject(_searchRequest);
            Debug.WriteLine("********json*************");
            Debug.WriteLine(json);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync("http://phoneofficine.it/niini-gim/lesson/search?sortedBy=number&order=asc&maxResult=100", content);
            if (response.IsSuccessStatusCode)
            {
                //await Navigation.PopAsync();
                await Application.Current.MainPage.DisplayAlert("Message", "Idea Added", "ok");
            }
            else
            {
                //DisplayAlert("Message", "Data Failed To Save", "Ok");
                await Application.Current.MainPage.DisplayAlert("Message", "Failed to add Idea", "ok");
            }
            var result = await response.Content.ReadAsStringAsync();
            Debug.WriteLine("********result*************");
            Debug.WriteLine(result);
            Ideas = JsonConvert.DeserializeObject<List<Client>>(result);
            Debug.WriteLine("********newRecord*************");
            Debug.WriteLine(Ideas);
            return Ideas;
        }
        */
        //Sportif
        public User CurrentUser { get; set; }
        private List<User> _clientAutoComplete;
        public List<User> ClientAutoComplete
        {
            get { return _clientAutoComplete; }
            set
            {
                _clientAutoComplete = value;
                OnPropertyChanged();
            }
        }
        public async Task<List<User>> ListClientsAutoComplete()
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

                ClientAutoComplete = (List<User>)response.Result;
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
                ClientAutoComplete = (List<User>)response.Result;
            }
            return ClientAutoComplete;
        }
        #endregion
    }
}
