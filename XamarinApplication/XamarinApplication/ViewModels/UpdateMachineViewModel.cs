using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
    public class UpdateMachineViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private Machine machine;
        #endregion

        #region Constructors
        public UpdateMachineViewModel(INavigation _navigation)
        {
            Navigation = _navigation;

            ListGYMAutoComplete();
            ListStatus = GetStatus().OrderBy(t => t.Value).ToList();
        }
        #endregion

        #region Properties
        public Machine Machine
        {
            get { return machine; }
            set
            {
                machine = value;
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
        public async void EditMachine()
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
            if (string.IsNullOrEmpty(Machine.code))
            {
                Value = true;
                return;
            }
            if (string.IsNullOrEmpty(Machine.serialNumber))
            {
                Value = true;
                return;
            }
            if (string.IsNullOrEmpty(Machine.ip))
            {
                Value = true;
                return;
            }
            if (Machine.gim == null)
            {
                Value = true;
                await Application.Current.MainPage.DisplayAlert("Alert", "GYM is Required", "ok");
                return;
            }
            if (SelectedStatus == null)
            {
                Value = true;
                await Application.Current.MainPage.DisplayAlert("Alert", "Status is Required", "ok");
                return;
            }

            var machine = new Machine
            {
                id = Machine.id,
                code = Machine.code,
                serialNumber = Machine.serialNumber,
                ip = Machine.ip,
                gim = Machine.gim,
                description = Machine.description,
                note = Machine.note,
                activationDate = Machine.activationDate,
                deliveryDate = Machine.deliveryDate,
                status = SelectedStatus.Key
            };
            await apiService.PutRequest<Machine>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/machine",
                 accesstoken,
                  machine);
            /*if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }*/
            Value = false;
            MachineViewModel.GetInstance().Update(machine);

            DependencyService.Get<INotification>().CreateNotification("GYM TECH", "Machine Updated");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
        #endregion

        #region Commands
        public ICommand UpdateMachine
        {
            get
            {
                return new Command(() =>
                {
                    EditMachine();
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
        #endregion

        #region Status
        public List<Language> ListStatus { get; set; }
        private Language _selectedStatus { get; set; }
        public Language SelectedStatus
        {
            get { return _selectedStatus; }
            set
            {
                if (_selectedStatus != value)
                {
                    _selectedStatus = value;
                    OnPropertyChanged();
                }
            }
        }
        public List<Language> GetStatus()
        {
            var languages = new List<Language>()
            {
                new Language(){Key =  "ON", Value= "In Ligne"},
                new Language(){Key =  "OF", Value= "Off Ligne"},
                new Language(){Key =  "ST", Value= "In Stand"}
            };

            return languages;
        }
        #endregion
    }
}
