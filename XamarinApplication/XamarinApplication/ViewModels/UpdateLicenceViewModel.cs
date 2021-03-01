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
    public class UpdateLicenceViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private Licence licence;
        #endregion

        #region Constructors
        public UpdateLicenceViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
        }
        #endregion

        #region Properties
        public Licence Licence
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
        public async void EditLicence()
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
            if (string.IsNullOrEmpty(Licence.number))
            {
                Value = true;
                return;
            }
            if (string.IsNullOrEmpty(Licence.name))
            {
                Value = true;
                return;
            }

            var licence = new Licence
            {
                id = Licence.id,
                number = Licence.number,
                name = Licence.name,
                startDate = Licence.startDate,
                endDate = Licence.endDate
            };
            await apiService.PutRequest<Licence>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/licence",
                 accesstoken,
                  licence);
            /*if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }*/
            Value = false;
            LicenceViewModel.GetInstance().Update(licence);

            DependencyService.Get<INotification>().CreateNotification("GYM TECH", "Licence Updated");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
        #endregion

        #region Commands
        public ICommand UpdateLicence
        {
            get
            {
                return new Command(() =>
                {
                    EditLicence();
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
    }
}
