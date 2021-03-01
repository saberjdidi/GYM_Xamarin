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
    public class AddGYMViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        #endregion

        #region Constructor
        public AddGYMViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
            apiService = new ApiServices();

            ListLicenceAutoComplete();
        }
        #endregion

        #region Properties
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string WebSite { get; set; }
        public string ReferencePerson { get; set; }
        private LicenceGYM licence = null;
        public LicenceGYM Licence
        {
            get { return licence; }
            set
            {
                licence = value;
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
        public async void AddGYM()
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
            if (string.IsNullOrEmpty(Name))
            {
                Value = true;
                return;
            }
            if (string.IsNullOrEmpty(Description))
            {
                Value = true;
                return;
            }
            if (Licence == null)
            {
                Value = true;
                return;
            }
            var gym = new AddGym
            {
                code = Code,
                name = Name,
                description = Description,
                tel = Telephone,
                email = Email,
                address = Address,
                webSite = WebSite,
                referencePerson = ReferencePerson,
                licence = Licence
            };
            var response = await apiService.Save<AddGym>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/gim/create",
                 accesstoken,
                 gym);

            if (!response.IsSuccess)
            {
            //await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }

            Value = false;
            DependencyService.Get<INotification>().CreateNotification("GYM TECH", "GYM Added");
            // await Navigation.PopPopupAsync();
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
        #endregion

        #region Commands
        public ICommand SaveGYM
        {
            get
            {
                return new Command(() =>
                {
                    AddGYM();
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
                    //  Navigation.PopAsync();
                    Debug.WriteLine("********Close*************");
                });
            }
        }
        #endregion

        #region Autocomplete
        //**************Licence**************
        private List<LicenceGYM> _licenceAutoComplete;
        public List<LicenceGYM> LicenceAutoComplete
        {
            get { return _licenceAutoComplete; }
            set
            {
                _licenceAutoComplete = value;
                OnPropertyChanged();
            }
        }
        public async Task<List<LicenceGYM>> ListLicenceAutoComplete()
        {
            var accesstoken = Settings.AccessToken;

            var response = await apiService.GetList<LicenceGYM>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/licence",
                 accesstoken);
            LicenceAutoComplete = (List<LicenceGYM>)response.Result;
            return LicenceAutoComplete;
        }
        #endregion
    }
}
