using GalaSoft.MvvmLight.Command;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.Services;
using XamarinApplication.Views;

namespace XamarinApplication.ViewModels
{
    public class ImagesExerciceViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        private DialogService dialogService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private Exercice exercice;
        private ObservableCollection<ImagesExercice> images;
        private List<ImagesExercice> imagesList;
        private bool isRefreshing;
        bool _isVisibleStatus;
        #endregion

        #region Constructors
        public ImagesExerciceViewModel()
        {
            dialogService = new DialogService();
            instance = this;
            GetImages();
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
        public ObservableCollection<ImagesExercice> Images
        {
            get { return images; }
            set { SetValue(ref this.images, value); }
        }
        public bool IsRefreshing
        {
            get
            {
                return isRefreshing;
            }
            set
            {
                SetValue(ref this.isRefreshing, value);
            }
        }
        public bool IsVisibleStatus
        {
            get { return _isVisibleStatus; }
            set
            {
                _isVisibleStatus = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Sigleton
        static ImagesExerciceViewModel instance;
        public static ImagesExerciceViewModel GetInstance()
        {
            if (instance == null)
            {
                return new ImagesExerciceViewModel();
            }

            return instance;
        }
        public void AddImage(ImagesExercice imagesExercice)
        {
            IsRefreshing = true;
            imagesList.Add(imagesExercice);
            Images = new ObservableCollection<ImagesExercice>(imagesList);
            IsRefreshing = false;
        }
        public async Task Delete(ImagesExercice imagesExercice)
        {
            IsRefreshing = true;
            var connection = await apiService.CheckConnection();
            var accesstoken = Settings.AccessToken;

            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var response = await apiService.Delete<ImagesExercice>(
                "http://phoneofficine.it",
                 "/niini-gim",
                 "/pictureExercise",
                imagesExercice.id,
                accesstoken);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            imagesList.Remove(imagesExercice);
            Images = new ObservableCollection<ImagesExercice>(imagesList);

            IsRefreshing = false;
        }
        #endregion

        #region Methods
        public async void GetImages()
        {
            IsRefreshing = true;
            var connection = await apiService.CheckConnection();
            var accesstoken = Settings.AccessToken;

            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Ok");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }
            var _exercice = new SeriesExercice
            {
                exercise = Exercice
            };

            var response = await apiService.SeriesData<ImagesExercice>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/pictureExercise/search?sortedBy=name&order=asc&maxResult=100",
                 accesstoken,
                 _exercice);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            imagesList = (List<ImagesExercice>)response.Result;
            Images = new ObservableCollection<ImagesExercice>(imagesList);
            IsRefreshing = false;
            if (Images.Count() == 0)
            {
                IsVisibleStatus = true;
            }
            else
            {
                IsVisibleStatus = false;
            }
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(GetImages);
            }
        }
        public Command UploadImage
        {
            get
            {
                return new Command(() =>
                {
                    PopupNavigation.Instance.PushAsync(new UploadImageExercicePage(Exercice));
                });
            }
        }
        #endregion
    }
}
