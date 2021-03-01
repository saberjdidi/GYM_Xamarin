using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
    public class UpdateProgramViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private Program program;
        #endregion

        #region Constructors
        public UpdateProgramViewModel(INavigation _navigation)
        {
            Navigation = _navigation;

            ListMachineAutoComplete();
            ListExerciceAutoComplete();
        }
        #endregion

        #region Properties
        public Program Program
        {
            get { return program; }
            set
            {
                program = value;
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
        #endregion

        #region Methods
        public async void EditProgram()
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
            if (string.IsNullOrEmpty(Program.name))
            {
                Value = true;
                return;
            }

            var program = new Program
            {
                id = Program.id,
                name = Program.name,
                machines = Program.machines,
                exercises = Program.exercises
            };
            await apiService.PutRequest<Program>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/program",
                 accesstoken,
                  program);
           /* if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }*/

            Value = false;
            ProgramViewModel.GetInstance().Update(program);

            DependencyService.Get<INotification>().CreateNotification("GYM TECH", "Program Updated");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
        #endregion

        #region Commands
        public ICommand UpdateProgram
        {
            get
            {
                return new Command(() =>
                {
                    EditProgram();
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
                    //App.Current.MainPage.Navigation.PopPopupAsync(true);
                    Debug.WriteLine("********Close*************");
                });
            }
        }
        #endregion

        #region Autocomplete
        //**************Machine**************
        private List<Machine> _machineAutoComplete;
        public List<Machine> MachineAutoComplete
        {
            get { return _machineAutoComplete; }
            set
            {
                _machineAutoComplete = value;
                OnPropertyChanged();
            }
        }
        public async Task<List<Machine>> ListMachineAutoComplete()
        {
            var accesstoken = Settings.AccessToken;

            var response = await apiService.GetList<Machine>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/machine",
                 accesstoken);
            MachineAutoComplete = (List<Machine>)response.Result;
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
    }
}
