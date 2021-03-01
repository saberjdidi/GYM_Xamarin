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
    public class AddProgramViewModel : INotifyPropertyChanged
    {
        #region Services
        private ApiServices apiService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private string _name;
        #endregion

        #region Constructor
        public AddProgramViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
            apiService = new ApiServices();
            SubmitCommand = new Command(Submit);

            SelectedExercice = new List<Exercice>();
            SelectedMachine = new List<Machine>();

            ListMachineAutoComplete();
            ListExerciceAutoComplete();

        }
        #endregion

        #region Properties
        public Command SubmitCommand { get; set; }
        public bool IsValidated { get; set; }
        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }
        
        private object selectedExercice;
        public object SelectedExercice
        {
            get { return selectedExercice; }
            set
            {
                selectedExercice = value;
               // RaisePropertyChanged("SelectedItem");
            }
        }
        private object selectedMachine;
        public object SelectedMachine
        {
            get { return selectedMachine; }
            set
            {
                selectedMachine = value;
                // RaisePropertyChanged("SelectedItem");
            }
        }

       /* private List<Exercice> _exercices = new List<Exercice>();
        public List<Exercice> Exercices
        {
            get { return _exercices; }
            set
            {
                _exercices = value;
                OnPropertyChanged();
            }
        }*/
        #endregion
        #region Methods
        public async void Submit()
        {
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
            if (!IsValidated)
            {
                await Application.Current.MainPage.DisplayAlert("", "You must fill all areas correctly!", "OK");
            }

            var program = new AddProgram
            {
                name = Name,
                machines = SelectedMachine,
               exercises = SelectedExercice
            };
            var response = await apiService.Save<AddProgram>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/program/create",
                 accesstoken,
                 program);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }

            DependencyService.Get<INotification>().CreateNotification("GYM TECH", "Program Added");
            // await Navigation.PopPopupAsync();
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
        #endregion

        #region Commands
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
        //**************Machine**************
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
        private List<Machine> _machineAutoComplete;
        public List<Machine> MachineAutoComplete
        {
            get { return _machineAutoComplete; }
            set
            {
                _machineAutoComplete = value;
                OnPropertyChanged("MachineAutoComplete");

            }
        }
        public async Task<List<Machine>> ListMachineAutoComplete()
        {
            //Current User
            var accesstoken = Settings.AccessToken;
            var username = Settings.Username;
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            var uri = "http://phoneofficine.it/niini-gim/user/username?username=" + username;
            var result = client.GetStringAsync(uri);
            User = JsonConvert.DeserializeObject<User>(result.Result);
            Debug.WriteLine("********User Menu********");
            Debug.WriteLine(User);

            if (User.role.name.Equals("ROLE_ADMIN"))
            {
                var response = await apiService.GetList<Machine>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/machine",
                 accesstoken);
                MachineAutoComplete = (List<Machine>)response.Result;
                //return MachineAutoComplete;
            }
            if (User.role.name.Equals("ROLE_ENTRENEUR"))
            {
                var _searchGym = new SearchByGym
                {
                    gim = User.gim
                };
                var response = await apiService.GymMachine<Machine>(
                     "http://phoneofficine.it",
                     "/niini-gim",
                     "/machine/searchPagination?sortedBy=code&order=asc&maxResult=100",
                     accesstoken,
                     _searchGym);
                MachineAutoComplete = (List<Machine>)response.Result;
            }
                return MachineAutoComplete;
        }
        //**************Exercice**************
        private List<Exercice> _exerciceAutoComplete;
        public List<Exercice> ExerciceAutoComplete
        {
            get { return _exerciceAutoComplete; }
            set
            {
                _exerciceAutoComplete = value;
                OnPropertyChanged();
            }
        }
        public async Task<List<Exercice>> ListExerciceAutoComplete()
        {
            var accesstoken = Settings.AccessToken;

            var response = await apiService.GetList<Exercice>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/exercice",
                 accesstoken);
            ExerciceAutoComplete = (List<Exercice>)response.Result;
            return ExerciceAutoComplete;
        }
        
        #endregion

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void RaisePropertyChanged(String name)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
