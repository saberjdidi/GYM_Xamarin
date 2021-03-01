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
    public class UpdateGYMViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private Gim gim;
        #endregion

        #region Constructors
        public UpdateGYMViewModel(INavigation _navigation)
        {
            Navigation = _navigation;

            ListLicenceAutoComplete();
        }
        #endregion

        #region Properties
        public Gim Gim
        {
            get { return gim; }
            set
            {
                gim = value;
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
        public async void EditGYM()
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
            if (string.IsNullOrEmpty(Gim.Code))
            {
                Value = true;
                return;
            }
            if (string.IsNullOrEmpty(Gim.Name))
            {
                Value = true;
                return;
            }
            if (string.IsNullOrEmpty(Gim.Description))
            {
                Value = true;
                return;
            }
            if (string.IsNullOrEmpty(Gim.licence.name))
            {
                Value = true;
                return;
            }

            var gim = new Gim
            {
                Id = Gim.Id,
                Code = Gim.Code,
                Name = Gim.Name,
                Description = Gim.Description,
                Address = Gim.Address,
                Tel = Gim.Tel,
                Email = Gim.Email,
                WebSite = Gim.WebSite,
                ReferencePerson = Gim.ReferencePerson,
                licence = Gim.licence
            };
            var response = await apiService.Put<Gim>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/gim",
                 accesstoken,
                  gim);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }

            Value = false;
            GimViewModel.GetInstance().Update(gim);

            DependencyService.Get<INotification>().CreateNotification("GYM TECH", "GYM Updated");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
        #endregion

        #region Commands
        public ICommand UpdateGYM
        {
            get
            {
                return new Command(() =>
                {
                    EditGYM();
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
