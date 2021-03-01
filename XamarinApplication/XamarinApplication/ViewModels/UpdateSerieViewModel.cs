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
    public class UpdateSerieViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private Serie serie;
        #endregion

        #region Constructors
        public UpdateSerieViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
        }
        #endregion

        #region Properties
        public Serie Serie
        {
            get { return serie; }
            set
            {
                serie = value;
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
        public async void EditSerie()
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
            if (Serie.serie == 0)
            {
                Value = true;
                await Application.Current.MainPage.DisplayAlert("Alert", "Serie is Required", "ok");
                return;
            }

            var serie = new Serie
            {
                id = Serie.id,
                serie = Serie.serie,
                initials = Serie.initials,
                effortIncrease = Serie.effortIncrease,
                secondsIncreaseEffort = Serie.secondsIncreaseEffort,
                limits = Serie.limits,
                timeStaticity = Serie.timeStaticity,
                exercise = Serie.exercise
            };
            var response = await apiService.Put<Serie>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/series",
                 accesstoken,
                  serie);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }

            Value = false;
            SeriesExerciceViewModel.GetInstance().Update(serie);

            DependencyService.Get<INotification>().CreateNotification("GYM TECH", "Serie Updated");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
        #endregion

        #region Commands
        public ICommand UpdateSerie
        {
            get
            {
                return new Command(() =>
                {
                    EditSerie();
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
