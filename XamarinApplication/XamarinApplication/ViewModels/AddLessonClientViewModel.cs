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
    public class AddLessonClientViewModel : BaseViewModel
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
        public AddLessonClientViewModel(User user)
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
        public int Number { get; set; }
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
        public async void AddLesson()
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

            var lesson = new AddClient
            {
                number = Number,
                startDate = Date,
                customerCard = User
            };
            await apiService.SaveRequest<AddClient>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/lesson/create",
                 accesstoken,
                 lesson);

            DependencyService.Get<INotification>().CreateNotification("GYM TECH", "Lesson Added");
            await Navigation.PopPopupAsync();
        }
        #endregion

        #region Commands
        public ICommand SaveLesson
        {
            get
            {
                return new Command(() =>
                {
                    AddLesson();
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
