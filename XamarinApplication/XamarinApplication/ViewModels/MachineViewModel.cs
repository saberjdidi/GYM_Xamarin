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
    public class MachineViewModel : BaseViewModel
    {
        #region Services
        private RestService restService;
        private ApiServices apiService;
        private DialogService dialogService;
        #endregion

        #region Attributes
        private ObservableCollection<Machine> machines;
        private bool isRefreshing;
        bool _isVisibleStatus;
        private bool _showHide = false;
        private string filter;
        private List<Machine> machinesList;
        #endregion

        #region Properties
        public ObservableCollection<Machine> Machines
        {
            get { return machines; }
            set
            {
                machines = value;
                OnPropertyChanged();
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
                isRefreshing = value;
                OnPropertyChanged();
            }
        }
        public string Filter
        {
            get { return filter; }
            set
            {
                filter = value;
                OnPropertyChanged();
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
        public MachineViewModel()
        {
            restService = new RestService();
            apiService = new ApiServices();
            dialogService = new DialogService();
            instance = this;
            LoadMachine();
        }
        #endregion

        #region Sigleton
        static MachineViewModel instance;
        public static MachineViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MachineViewModel();
            }

            return instance;
        }

        public void Update(Machine machine)
        {
            IsRefreshing = true;
            var oldmachine = machinesList
                .Where(p => p.id == machine.id)
                .FirstOrDefault();
            oldmachine = machine;
            Machines = new ObservableCollection<Machine>(machinesList);
            IsRefreshing = false;
        }
        public async Task Delete(Machine machine)
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

            var response = await apiService.Delete<Machine>(
                "http://phoneofficine.it",
                 "/niini-gim",
                 "/machine",
                machine.id,
                accesstoken);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            machinesList.Remove(machine);
            Machines = new ObservableCollection<Machine>(machinesList);

            IsRefreshing = false;
        }
        #endregion

        #region Methods
        public async void LoadMachine()
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

            var response = await restService.GetList<Machine>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/machine",
                 accesstoken);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            machinesList = (List<Machine>)response.Result;
            Machines = new ObservableCollection<Machine>(machinesList);
            IsRefreshing = false;
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadMachine);
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
                Machines = new ObservableCollection<Machine>(machinesList);
                IsVisibleStatus = false;
            }
            else
            {
                Machines = new ObservableCollection<Machine>(
                    machinesList.Where(
                        l => l.code.ToLower().Contains(Filter.ToLower()) ||
                             l.serialNumber.ToLower().Contains(Filter.ToLower())));
                if (Machines.Count() == 0)
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
