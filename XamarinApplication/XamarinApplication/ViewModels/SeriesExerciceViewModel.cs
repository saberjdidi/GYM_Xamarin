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
    public class SeriesExerciceViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        private DialogService dialogService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private Exercice exercice;
        private ObservableCollection<Serie> series;
        private List<Serie> seriesList;
        private bool isRefreshing;
        bool _isVisibleStatus;
        #endregion

        #region Constructors
        public SeriesExerciceViewModel()
        {
            dialogService = new DialogService();
            instance = this;
            GetSeries();
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
        public ObservableCollection<Serie> Series
        {
            get { return series; }
            set { SetValue(ref this.series, value); }
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
        static SeriesExerciceViewModel instance;
        public static SeriesExerciceViewModel GetInstance()
        {
            if (instance == null)
            {
                return new SeriesExerciceViewModel();
            }

            return instance;
        }

        public void Update(Serie serie)
        {
            IsRefreshing = true;
            var oldserie = seriesList
                .Where(p => p.id == serie.id)
                .FirstOrDefault();
            oldserie = serie;
            Series = new ObservableCollection<Serie>(seriesList);
            IsRefreshing = false;
        }
        public async Task Delete(Serie serie)
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

            var response = await apiService.Delete<Serie>(
                "http://phoneofficine.it",
                 "/niini-gim",
                 "/series",
                serie.id,
                accesstoken);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            seriesList.Remove(serie);
            Series = new ObservableCollection<Serie>(seriesList);

            IsRefreshing = false;
        }
        #endregion

        #region Methods
        public async void GetSeries()
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

            var response = await apiService.SeriesData<Serie>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/series/search?sortedBy=serie&order=asc&maxResult=100",
                 accesstoken,
                 _exercice);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            seriesList = (List<Serie>)response.Result;
            Series = new ObservableCollection<Serie>(seriesList);
            IsRefreshing = false;
            if (Series.Count() == 0)
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
                return new RelayCommand(GetSeries);
            }
        }
        public Command AddSerie
        {
            get
            {
                return new Command(() =>
                {
                    PopupNavigation.Instance.PushAsync(new AddSeriePage(Exercice));
                });
            }
        }
        #endregion
    }
}
