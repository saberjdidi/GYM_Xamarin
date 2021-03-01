using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
    public class AddExerciceViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        #endregion

        #region Constructor
        public AddExerciceViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
            apiService = new ApiServices();

           ListExerciceTypologyAutoComplete();
            ListLevel = GetLevels().OrderBy(t => t.Value).ToList();

        }
        #endregion

        #region Properties
        public string Name { get; set; }
        public int TotalSerie { get; set; }
        public int TotalReputation { get; set; }
        public int BreakDuration { get; set; }
        public long Speed { get; set; }
        private ExerciceTypology _exerciceTypology = null;
        public ExerciceTypology ExerciceTypology
        {
            get { return _exerciceTypology; }
            set
            {
                _exerciceTypology = value;
                OnPropertyChanged();
            }
        }
        private bool _concentric = false;
        public bool Concentric
        {
            get { return _concentric; }
            set
            {
                _concentric = value;
                OnPropertyChanged();
            }
        }
        private bool _eccentric = false;
        public bool Eccentric
        {
            get { return _eccentric; }
            set
            {
                _eccentric = value;
                OnPropertyChanged();
            }
        }
        public int InitialLood { get; set; }
        public int SecondLood { get; set; }
        public int InitialLoodEccentric { get; set; }
        public int SecondLoodEccentric { get; set; }

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
        private bool _showHide = false;
        public bool ShowHide
        {
            get => _showHide;
            set
            {
                _showHide = value;
                OnPropertyChanged();
            }
        }
        #endregion 

        #region Methods
        public async void AddExercice()
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
            if (string.IsNullOrEmpty(Name))
            {
                Value = true;
                return;
            }
            if (ExerciceTypology == null)
            {
                Value = true;
                return;
            }
            if (SelectedLevel == null)
            {
                Value = true;
                await Application.Current.MainPage.DisplayAlert("Alert", "Level is Required", "ok");
                return;
            }
            if(InitialLood == 0)
            {
                Value = true;
                return;
            }

            var exercice = new AddExercice
            {
                level = SelectedLevel.Key,
                name = Name,
                exerciseTypologie = ExerciceTypology,
                totalSeries = TotalSerie,
                totalReputation = TotalReputation,
                breakDuration = BreakDuration,
                speed = Speed,
                concentric = Concentric,
                lood = InitialLood,
                secondLood = SecondLood,
                eccentric = Eccentric,
                loodEccentric = InitialLoodEccentric,
                secondLoodEccentric = SecondLoodEccentric
            };
            var response = await apiService.Save<AddExercice>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/exercice/create",
                 accesstoken,
                 exercice);

             if (!response.IsSuccess)
             {
                 await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                 return;
             }

            Value = false;
            DependencyService.Get<INotification>().CreateNotification("GYM TECH", "Exercice Added");
            await Navigation.PopPopupAsync();
        }
        #endregion

        #region Commands
        public ICommand SaveExercice
        {
            get
            {
                return new Command(() =>
                {
                    AddExercice();
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

        public ICommand OpenSearchBar
        {
            get
            {
                return new Command(() =>
                {
                    if (Concentric == true && ShowHide == false)
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

        #region Autocomplete
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

        #region Level
        public List<Language> ListLevel { get; set; }
        private Language _selectedLevel { get; set; }
        public Language SelectedLevel
        {
            get { return _selectedLevel; }
            set
            {
                if (_selectedLevel != value)
                {
                    _selectedLevel = value;
                    OnPropertyChanged();
                }
            }
        }
        public List<Language> GetLevels()
        {
            var languages = new List<Language>()
            {
                new Language(){Key =  "BI", Value= "Beginner"},
                new Language(){Key =  "IN", Value= "Intermediate"},
                new Language(){Key =  "XR", Value= "Expert"}
            };

            return languages;
        }
        #endregion
    }
}
