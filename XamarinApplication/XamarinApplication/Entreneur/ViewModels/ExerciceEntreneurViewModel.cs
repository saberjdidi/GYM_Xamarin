using GalaSoft.MvvmLight.Command;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.Services;
using XamarinApplication.ViewModels;
using XamarinApplication.Views;

namespace XamarinApplication.Entreneur.ViewModels
{
    public class ExerciceEntreneurViewModel : BaseViewModel
    {
        #region Services
        private RestService restService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private ObservableCollection<Exercice> exercices;
        //private InfiniteScrollCollection<Exercice> exercices;
        private bool isRefreshing;
        bool _isVisibleStatus;
        private List<Exercice> exerciceList;

        private bool _isBusy;
        private const int _maxResult = 8;
        int _offset = 0;
        #endregion

        #region Properties
        public ObservableCollection<Exercice> Exercices
        {
            get => this.exercices;
            set => this.SetValue(ref this.exercices, value);
        }
        public bool IsRefreshing
        {
            get => this.isRefreshing;
            set => this.SetValue(ref this.isRefreshing, value);
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
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructors
        public ExerciceEntreneurViewModel()
        {
            restService = new RestService();

            LoadExercices();

        }
        #endregion

        #region Methods
        private async void LoadExercices()
        {
            IsRefreshing = true;
            var connection = await restService.CheckConnection();
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
            var _searchRequest = new SearchRequest
            {
            };

            var response = await restService.PostData<Exercice>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/exercice/searchPagination?sortedBy=exerciseTypologie&order=asc&maxResult=16",
                 accesstoken,
                 _searchRequest);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            exerciceList = (List<Exercice>)response.Result;
            Exercices = new ObservableCollection<Exercice>(exerciceList);
            IsRefreshing = false;
        }
        public async void LoadMoreItems(Exercice currentItem)
        {
            int itemIndex = Exercices.IndexOf(currentItem);
            var accesstoken = Settings.AccessToken;
            var connection = await restService.CheckConnection();
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

            _offset = Exercices.Count;

            if (Exercices.Count - 8 == itemIndex)
            {
                IsBusy = true;
                IsRefreshing = true;
                var _searchRequest = new SearchRequest
                {
                };
                var response = await restService.LoadMoreData<Exercice>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/exercice/searchPagination?sortedBy=exerciseTypologie&order=asc&maxResult=8&firstResult=" + _offset,
                 accesstoken,
                 _searchRequest);
                if (!response.IsSuccess)
                {
                    IsRefreshing = true;
                    await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                    return;
                }
                exerciceList = (List<Exercice>)response.Result;
                foreach (Exercice item in exerciceList)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Exercices.Add(item);
                        IsBusy = false;
                        IsRefreshing = false;
                    });
                }
                IsBusy = false;
                IsRefreshing = false;
            }
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadExercices);
            }
        }
        public ICommand SearchPopup
        {
            get
            {
                return new Command(async () =>
                {

                    MessagingCenter.Subscribe<DialogResultExercice>(this, "PopUpDataExercice", (value) =>
                    {
                        Exercices = value.ExercicesPopup;
                        IsBusy = false;
                        IsRefreshing = false;
                        if (Exercices.Count() == 0)
                        {
                            IsVisibleStatus = true;
                        }
                        else
                        {
                            IsVisibleStatus = false;
                        }
                    });
                    await PopupNavigation.Instance.PushAsync(new SearchExercicePage());
                });
            }
        }
        #endregion
    }
}
