using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
    public class UpdateParameterClientViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private ParametreSportif _parameters;
        #endregion

        #region Constructors
        public UpdateParameterClientViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
        }
        #endregion

        #region Properties
        public ParametreSportif Parameters
        {
            get { return _parameters; }
            set
            {
                _parameters = value;
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
        public async void EditParameter()
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

            var parametreSportif = new ParametreSportif
            {
                id = Parameters.id,
                date = Parameters.date,
                chest = Parameters.chest,
                rightArm = Parameters.rightArm,
                leftArm = Parameters.leftArm,
                life = Parameters.life,
                sides = Parameters.sides,
                thighRight = Parameters.thighRight,
                thighLeft = Parameters.thighLeft,
                weight = Parameters.weight,
                sportif = Parameters.sportif
            };
            await apiService.PutRequest<ParametreSportif>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/circumference",
                 accesstoken,
                  parametreSportif);

            ParametresClientViewModel.GetInstance().Update(parametreSportif);

            DependencyService.Get<INotification>().CreateNotification("GYM TECH", "Parameter Updated");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
        #endregion

        #region Commands
        public ICommand UpdateParameter
        {
            get
            {
                return new Command(() =>
                {
                    EditParameter();
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
    }
}
