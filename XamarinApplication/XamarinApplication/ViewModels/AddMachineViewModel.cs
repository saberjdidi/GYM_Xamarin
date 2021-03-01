using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
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
    public class AddMachineViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        #endregion

        #region Constructor
        public AddMachineViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
            apiService = new ApiServices();

            ListGYMAutoComplete();
            ListStatus = GetStatus().OrderBy(t => t.Value).ToList();
        }
        #endregion

        #region Properties
        public string Code { get; set; }
        public string SerialNumber { get; set; }
        public string IP { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        //public string Status { get; set; }
        private Gim gim = null;
        public Gim Gim
        {
            get { return gim; }
            set
            {
                gim = value;
                OnPropertyChanged();
            }
        }
        private DateTime _deliveryDate = System.DateTime.Today;  //DateTime.Today.Date;
        public DateTime DeliveryDate
        {
            get { return _deliveryDate; }
            set
            {
                _deliveryDate = value;
                OnPropertyChanged();
            }
        }
        private DateTime _activityStartDate = System.DateTime.Today;  //DateTime.Today.Date;
        public DateTime ActivityStartDate
        {
            get { return _activityStartDate; }
            set
            {
                _activityStartDate = value;
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
        public async void AddMachine()
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
            if (string.IsNullOrEmpty(Code))
            {
                Value = true;
                return;
            }
            if (string.IsNullOrEmpty(SerialNumber))
            {
                Value = true;
                return;
            }
            if (string.IsNullOrEmpty(IP))
            {
                Value = true;
                return;
            }
            if (Gim == null)
            {
                Value = true;
                return;
            }
            if (SelectedStatus == null)
            {
                Value = true;
                await Application.Current.MainPage.DisplayAlert("Alert", "Status is Required", "ok");
                return;
            }

            var machine = new AddMachine
            {
                status = SelectedStatus.Key,
                code = Code,
                serialNumber = SerialNumber,
                ip = IP,
                description = Description,
                note = Note,
                gim = Gim,
                deliveryDate = DeliveryDate,
                activationDate = ActivityStartDate
            };
            await apiService.SaveRequest<AddMachine>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/machine/create",
                 accesstoken,
                 machine);

            /* if (!response.IsSuccess)
             {
                 await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                 return;
             }*/

            Value = false;
            DependencyService.Get<INotification>().CreateNotification("GYM TECH", "Machine Added");
            await Navigation.PopPopupAsync();
        }
        #endregion

        #region Commands
        public ICommand SaveMachine
        {
            get
            {
                return new Command(() =>
                {
                    AddMachine();
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
