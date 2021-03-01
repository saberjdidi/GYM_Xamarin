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
    public class AddSerieViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private Exercice exercice;
        private bool value = false;
        #endregion

        #region Constructor
        public AddSerieViewModel(Exercice exercice)
        {
            apiService = new ApiServices();

        }
        #endregion

        #region Properties
        public Exercice Exercice
        {
            get { return exercice; }
            set
            {
                exercice = value;
                OnPropertyChanged();
            }
        }
        public long Serie { get; set; }
        public long Initials { get; set; }
        public long EffortIncrease { get; set; }
        public long SecondIncreaseEffort { get; set; }
        public long Limit { get; set; }
        public long StaticityTime { get; set; }
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
        public async void AddSerie()
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
            if (Serie == 0)
            {
                Value = true;
                await Application.Current.MainPage.DisplayAlert("Alert", "Serie is Required", "ok");
                return;
            }

            var serie = new AddSerie
            {
                serie = Serie,
                initials = Initials,
                effortIncrease = EffortIncrease,
                secondsIncreaseEffort = SecondIncreaseEffort,
                limits = Limit,
                timeStaticity = StaticityTime,
                exercise = Exercice
            };
            var response = await apiService.Save<AddSerie>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/series/create",
                 accesstoken,
                 serie);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }

            Value = false;
            DependencyService.Get<INotification>().CreateNotification("GYM TECH", "Serie Added");
            await Navigation.PopPopupAsync();
        }
        #endregion

        #region Commands
        public ICommand SaveSerie
        {
            get
            {
                return new Command(() =>
                {
                    AddSerie();
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
