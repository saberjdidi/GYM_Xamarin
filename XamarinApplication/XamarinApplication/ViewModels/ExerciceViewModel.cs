using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.Services;
using XamarinApplication.Views;

namespace XamarinApplication.ViewModels
{
    public class ExerciceViewModel : BaseViewModel
    {
        #region Services
        private RestService restService;
        private ApiServices apiService;
        private DialogService dialogService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private ObservableCollection<Exercice> exercices;
        //private InfiniteScrollCollection<Exercice> exercices;
        private bool isRefreshing;
        bool _isVisibleStatus;
        private bool _showHide = false;
        private string filter;
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
        /* public InfiniteScrollCollection<Exercice> Exercices
          {
              get => this.exercices;
              set => this.SetValue(ref this.exercices, value);
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
             */
        public bool IsRefreshing
        {
            get => this.isRefreshing;
            set => this.SetValue(ref this.isRefreshing, value);
        }

        public string Filter
        {
            get { return this.filter; }
            set
            {
                SetValue(ref this.filter, value);
                Search();
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
        public bool ShowHide
        {
            get => _showHide;
            set
            {
                _showHide = value;
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
        public ExerciceViewModel()
        {
            restService = new RestService();
            apiService = new ApiServices();
            dialogService = new DialogService();
            instance = this;

            LoadExercices();

            /* Exercices = new InfiniteScrollCollection<Exercice>
             {
                 OnLoadMore = async () =>
                 {
                     IsBusy = true;

                     // load the next page
                     var page = Exercices.Count / PageSize;

                     var accesstoken = Settings.AccessToken;
                     var response = await restService.GetList<Exercice>(
                  "http://phoneofficine.it",
                  "/niini-gim",
                  "/exercice",
                  page,
                  PageSize,
                  accesstoken);
                     exerciceList = (List<Exercice>)response.Result;

                     IsBusy = false;

                     return exerciceList;
                 },
                 OnCanLoadMore = () =>
                 {
                     return Exercices.Count < 100;
                 }
             };
             */
        }
        #endregion

        #region Sigleton
        static ExerciceViewModel instance;
        public static ExerciceViewModel GetInstance()
        {
            if (instance == null)
            {
                return new ExerciceViewModel();
            }

            return instance;
        }

        public void Update(Exercice exercice)
        {
            IsRefreshing = true;
            var oldexercice = exerciceList
                .Where(p => p.id == exercice.id)
                .FirstOrDefault();
            oldexercice = exercice;
            Exercices = new ObservableCollection<Exercice>(exerciceList);
            IsRefreshing = false;
        }
        public async Task Delete(Exercice exercice)
        {
            IsRefreshing = true;
            var connection = await restService.CheckConnection();
            var accesstoken = Settings.AccessToken;

            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var response = await apiService.Delete<Exercice>(
                "http://phoneofficine.it",
                 "/niini-gim",
                 "/exercice",
                exercice.id,
                accesstoken);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            exerciceList.Remove(exercice);
            Exercices = new ObservableCollection<Exercice>(exerciceList);

            IsRefreshing = false;
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
            /*var response = await restService.GetList<Exercice>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/exercice",
                 pageIndex: 0, 
                 pageSize: PageSize,
                 accesstoken);
                 var response = await restService.GetList<Exercice>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/exercice",
                 accesstoken);
                 */

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
           // Exercices.AddRange(exerciceList);
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
                    //IsVisible = true;
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
        /*  //second method
        public async void LoadExercice()
        {
            using (var client = new HttpClient())
            {
                // send a GET request with authorization 
                var accesstoken = Settings.AccessToken;
                var connection = await restService.CheckConnection();
                IsRefreshing = true;

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

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
                var uri = "http://phoneofficine.it/niini-gim/exercice";
                var result = await client.GetStringAsync(uri);
                //handling the answer  
                exerciceList = JsonConvert.DeserializeObject<List<Exercice>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                Exercices = new ObservableCollection<Exercice>(exerciceList);
                IsRefreshing = false;
            }
        }*/
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
        public ICommand SearchCommand
        {
            get
            {
                ShowHide = false;
                return new RelayCommand(Search);
            }
        }
        private void Search()
        {
            if (string.IsNullOrEmpty(Filter))
            {
                Exercices = new ObservableCollection<Exercice>(exerciceList);
                IsVisibleStatus = false;
            }
            else
            {
                Exercices = new ObservableCollection<Exercice>(
                    exerciceList.Where(
                        l =>  l.name.ToLower().StartsWith(Filter.ToLower())));

                if (Exercices.Count() == 0)
                {
                    IsVisibleStatus = true;
                }
                else
                {
                    IsVisibleStatus = false;
                }
            }
        }
        public ICommand OpenSearchBar
        {
            get
            {
                return new Command(() =>
                {
                    if (ShowHide == false)
                    {
                        ShowHide = true;
                    }
                    else
                    {
                        ShowHide = false;
                    }
                });
            }
        }
        #endregion
    }
}
