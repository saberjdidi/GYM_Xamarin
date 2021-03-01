using GalaSoft.MvvmLight.Command;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
    public class AddBiaClientViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private User _user;
        private ImageSource imageSource;
        private MediaFile file;
        #endregion

        #region Constructor
        public AddBiaClientViewModel(User user)
        {
            apiService = new ApiServices();
            this.ImageSource = "noImage";

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
        public string Note { get; set; }
        public ImageSource ImageSource
        {
            get => this.imageSource;
            set => this.SetValue(ref this.imageSource, value);
        }
        #endregion

        #region Methods
        public async void AddBia()
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
            if(file == null)
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Please Select File", "ok");
                return;
            }

           /* string imagePath = file.Path;
            string filename = imagePath.Substring(imagePath.LastIndexOf("/") + 1);
            byte[] fileByte = Convert.FromBase64String(filename);*/

            Stream stream = file.GetStream();
            byte[] buf = new byte[stream.Length];  //declare arraysize
            stream.Read(buf, 0, buf.Length); // read from stream to byte array

            var biaSportif = new AddBiaSportif
            {
                date = Date,
                note = Note,
                file = buf,
                sportif = User
            };
            await apiService.SaveRequest<AddBiaSportif>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/bia/create",
                 accesstoken,
                 biaSportif);

            DependencyService.Get<INotification>().CreateNotification("GYM TECH", "Bia Added");
            await Navigation.PopPopupAsync();
        }

        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();

            var source = await Application.Current.MainPage.DisplayActionSheet(
                "Upload file !",
                "Cancel",
                null,
                "From Gallery");

            if (source == "Cancel")
            {
                this.file = null;
                return;
            }
            if (source == "From Gallery")
            {
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (this.file != null)
            {
                this.ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }
        }
        #endregion

        #region Commands
        public ICommand ChangeImageCommand => new RelayCommand(this.ChangeImage);
        public ICommand SaveBia
        {
            get
            {
                return new Command(() =>
                {
                    AddBia();
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
