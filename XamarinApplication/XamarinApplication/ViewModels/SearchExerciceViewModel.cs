using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
    public class SearchExerciceViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        #endregion

        #region Attributes
        private ObservableCollection<Exercice> exercices;
        private bool isRefreshing;
        private bool isVisible;
        private List<Exercice> exerciceList;
        private bool _isBusy;
        private const int _maxResult = 100;
        public EventHandler<DialogResultExercice> OnDialogClosed;
        private string _name = "";
        private ExerciceTypology _exerciceTypology = null;
        #endregion

        #region Constructors
        public SearchExerciceViewModel()
        {
            apiService = new ApiServices();
            ListExerciceAutoComplete();
            ListExerciceTypologyAutoComplete();
        }
        #endregion

        #region Properties
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public ExerciceTypology ExerciceTypology
        {
            get { return _exerciceTypology; }
            set
            {
                _exerciceTypology = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Exercice> Exercices
        {
            get => this.exercices;
            set => this.SetValue(ref this.exercices, value);
        }
        public bool IsRefreshing
        {
            get
            {
                return isRefreshing;
            }
            set
            {
                isRefreshing = value;
                OnPropertyChanged();
            }
        }
        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                this.isVisible = value;
                //this.RaisePropertyChanged("IsVisible");
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

        #region Methods
        public async void SearchExercice()
        {
            IsRefreshing = true;
            //IsVisible = true;
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
            var _searchRequest = new SearchExercice
            {
                name = Name,
                exerciseTypologie = ExerciceTypology
            };

            var response = await apiService.SearchExerciceRequest<Exercice>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/exercice/searchPagination?sortedBy=exerciseTypologie&order=asc&maxResult=200",
                 accesstoken,
                 _searchRequest);
            exerciceList = (List<Exercice>)response.Result;
            Exercices = new ObservableCollection<Exercice>(exerciceList);

            MessagingCenter.Send(new DialogResultExercice() { ExercicesPopup = Exercices }, "PopUpDataExercice");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);

            IsVisible = false;
            IsRefreshing = false;
        }
        #endregion

        #region Commands
        public ICommand SearchCommand
        {
            get
            {
                return new Command(() =>
                {
                    SearchExercice();
                });
            }
        }
        public ICommand RefreshCommand
        {

            get
            {
                return new Command(() =>
                {
                    SearchExercice();
                });
            }
        }
        #endregion

        #region Autocomplete
        //**************Exercice**************
        private List<Exercice> _exerciceAutoComplete;
        public List<Exercice> ExerciceAutoComplete
        {
            get { return _exerciceAutoComplete; }
            set
            {
                _exerciceAutoComplete = value;
                OnPropertyChanged();
            }
        }
        public async Task<List<Exercice>> ListExerciceAutoComplete()
        {
            var accesstoken = Settings.AccessToken;

            var response = await apiService.GetList<Exercice>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/exercice",
                 accesstoken);
            ExerciceAutoComplete = (List<Exercice>)response.Result;
            return ExerciceAutoComplete;
        }
        //**************ExerciceTypology**************
        private List<ExerciceTypology> _exerciceTypologyAutoComplete;
        public List<ExerciceTypology> ExerciceTypologyAutoComplete
        {
            get { return _exerciceTypologyAutoComplete; }
            set
            {
                _exerciceTypologyAutoComplete = value;
                OnPropertyChanged();
            }
        }
        public async Task<List<ExerciceTypology>> ListExerciceTypologyAutoComplete()
        {
            var accesstoken = Settings.AccessToken;

            var response = await apiService.GetList<ExerciceTypology>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/exerciseTypologie",
                 accesstoken);
            ExerciceTypologyAutoComplete = (List<ExerciceTypology>)response.Result;
            return ExerciceTypologyAutoComplete;
        }
        #endregion
    }

    public class DialogResultExercice
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ObservableCollection<Exercice> ExercicesPopup { get; set; }
    }
}
