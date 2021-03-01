using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
    public class AddLicenceViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private bool value = false;
        #endregion

        #region Constructor
        public AddLicenceViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
            apiService = new ApiServices();
        }
        #endregion

        #region Properties
        public string Number { get; set; }
        public string Name { get; set; }
        private DateTime _startDate = System.DateTime.Today;  //System.DateTime.UtcNow;
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }
        private DateTime _endDate = System.DateTime.Today;  //DateTime.Today.Date;
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                OnPropertyChanged();
            }
        }
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
        public async void AddLicence()
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
            if (string.IsNullOrEmpty(Number))
            {
                Value = true;
                return;
            }
            if (string.IsNullOrEmpty(Name))
            {
                Value = true;
                return;
            }

            var licence = new AddLicence
            {
                number = Number,
                name = Name,
                startDate = StartDate,
                endDate = EndDate
            };
            await apiService.SaveRequest<AddLicence>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/licence/create",
                 accesstoken,
                 licence);

           /* if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }*/

            Value = false;
            DependencyService.Get<INotification>().CreateNotification("GYM TECH", "Licence Added");
            await Navigation.PopPopupAsync();
        }
        #endregion

        #region Commands
        public ICommand SaveLicence
        {
            get
            {
                return new Command(() =>
                {
                    AddLicence();
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
    }
}
