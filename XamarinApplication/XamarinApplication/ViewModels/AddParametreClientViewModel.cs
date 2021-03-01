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
    public class AddParametreClientViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private User _user;
        private bool value = false;
        #endregion

        #region Constructor
        public AddParametreClientViewModel(User user)
        {
            apiService = new ApiServices();

        }
        #endregion

        #region Properties
        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }
        private DateTime _date = System.DateTime.Today; 
        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged();
            }
        }
        public int Chest { get; set; }
        public int Life { get; set; }
        public int RightArm { get; set; }
        public int LeftArm { get; set; }
        public int ThighRight { get; set; }
        public int ThighLeft { get; set; }
        public int Sides { get; set; }
        public int Weight { get; set; } 
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
        public async void AddParametre()
        {
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

            var parametreSportif = new AddParametreSportif
            {
                date = Date,
                chest = Chest,
                rightArm = RightArm,
                leftArm = LeftArm,
                life = Life,
                sides = Sides,
                thighRight = ThighRight,
                thighLeft = ThighLeft,
                weight = Weight,
                sportif = User
            };
            await apiService.SaveRequest<AddParametreSportif>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/circumference/create",
                 accesstoken,
                 parametreSportif);

            DependencyService.Get<INotification>().CreateNotification("GYM TECH", "Parametre Added");
            await Navigation.PopPopupAsync();
        }
        #endregion

        #region Commands
        public ICommand SaveParametre
        {
            get
            {
                return new Command(() =>
                {
                    AddParametre();
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
