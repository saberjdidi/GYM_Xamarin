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
    public class UpdateBiaClientViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private BiaSportif _bia;
        private ImageSource imageSource;
        #endregion

        #region Constructors
        public UpdateBiaClientViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
            this.ImageSource = "noImage";
        }
        #endregion

        #region Properties
        public BiaSportif Bia
        {
            get { return _bia; }
            set
            {
                _bia = value;
                OnPropertyChanged();
            }
        }
        public ImageSource ImageSource
        {
            get => this.imageSource;
            set => this.SetValue(ref this.imageSource, value);
        }
        #endregion

        #region Methods
        public async void EditBia()
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

            var bia = new BiaSportif
            {
                id = Bia.id,
                date = Bia.date,
                note = Bia.note,
                sportif = Bia.sportif
            };
            await apiService.PutRequest<BiaSportif>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/bia",
                 accesstoken,
                  bia);

            BiaClientViewModel.GetInstance().Update(bia);

            DependencyService.Get<INotification>().CreateNotification("GYM TECH", "Bia Updated");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
        #endregion

        #region Commands
        public ICommand UpdateBia
        {
            get
            {
                return new Command(() =>
                {
                    EditBia();
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
