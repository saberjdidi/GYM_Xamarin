using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
    public class ProgramViewModel : BaseViewModel
    {
        #region Services
        private RestService restService;
        private ApiServices apiService;
        private DialogService dialogService;
        #endregion

        #region Attributes

        private ObservableCollection<Program> programs;
        //private ObservableCollection<ProgramItemViewModel> programs;
        private bool isRefreshing;
        bool _isVisibleStatus;
        private bool _showHide = false;
        private string filter;
        private List<Program> programsList;
        #endregion

        #region Properties
        /*public ObservableCollection<ProgramItemViewModel> Programs
        {
            get { return this.programs; }
            set { SetValue(ref this.programs, value); }
        }*/
        public ObservableCollection<Program> Programs
        {
            get { return this.programs; }
            set { SetValue(ref this.programs, value); }
        }

        public bool IsRefreshing
        {
            get
            {
                return this.isRefreshing;
            }
            set
            {
                SetValue(ref this.isRefreshing, value);
            }
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

        #region Constructors
        public ProgramViewModel()
        {
            restService = new RestService();
            apiService = new ApiServices();
            dialogService = new DialogService();
            instance = this;
            LoadProgram();
        }
        #endregion

        #region Sigleton
        static ProgramViewModel instance;
        public static ProgramViewModel GetInstance()
        {
            if (instance == null)
            {
                return new ProgramViewModel();
            }

            return instance;
        }

        public void Update(Program program)
        {
            IsRefreshing = true;
            var oldprogram = programsList
                .Where(p => p.id == program.id)
                .FirstOrDefault();
            oldprogram = program;
            Programs = new ObservableCollection<Program>(programsList);
            IsRefreshing = false;
        }
        public async Task Delete(Program program)
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

            var response = await apiService.Delete<Program>(
                "http://phoneofficine.it",
                 "/niini-gim",
                 "/program",
                program.id,
                accesstoken);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            programsList.Remove(program);
            Programs = new ObservableCollection<Program>(programsList);

            IsRefreshing = false;
        }
        #endregion

        #region Methods
        public async void LoadProgram()
        {
            IsRefreshing = true;
            var connection = await restService.CheckConnection();
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

            var response = await restService.GetList<Program>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/program",
                 accesstoken);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            programsList = (List<Program>)response.Result;
            Programs = new ObservableCollection<Program>(programsList);
            // MainViewModel.GetInstance().ProgramsList = (List<Program>)response.Result; //first method
            //this.Programs = new ObservableCollection<ProgramItemViewModel>(this.ToProgramItemViewModel()); //first method
            IsRefreshing = false;
        }
        #endregion
        #region Methods2
        /*private IEnumerable<ProgramItemViewModel> ToProgramItemViewModel()
        {
            return MainViewModel.GetInstance().ProgramsList.Select(p => new ProgramItemViewModel
            {
                id = p.id,
                name = p.name,
                machine = p.machine,
                exercises = p.exercises,
                coach = p.coach,
            });
        }*/
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadProgram);
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
                Programs = new ObservableCollection<Program>(programsList);
                IsVisibleStatus = false;
            }
            else
            {
                Programs = new ObservableCollection<Program>(
                    programsList.Where(
                        p => p.name.ToLower().Contains(Filter.ToLower())));

                if (Programs.Count() == 0)
                {
                    IsVisibleStatus = true;
                }
                else
                {
                    IsVisibleStatus = false;
                }
            }
        }
        /* private void Search()
         {
             if (string.IsNullOrEmpty(Filter))
             {
                 Programs = new ObservableCollection<ProgramItemViewModel>(ToProgramItemViewModel());
             }
             else
             {
                 Programs = new ObservableCollection<ProgramItemViewModel>(
                     ToProgramItemViewModel().Where(
                         p => p.name.ToLower().Contains(Filter.ToLower())));
             }
         }*/
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
