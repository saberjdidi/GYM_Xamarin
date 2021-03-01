using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
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
using XamarinApplication.Views;

namespace XamarinApplication.ViewModels
{
    public class UpdateExerciceViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private Exercice exercice;
        #endregion

        #region Constructors
        public UpdateExerciceViewModel(INavigation _navigation)
        {
            Navigation = _navigation;

            ListExerciceTypologyAutoComplete();
            ListLevel = GetLevels().OrderBy(t => t.Value).ToList();
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
        public async void EditExercice()
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
            if (string.IsNullOrEmpty(Exercice.name))
            {
                Value = true;
                return;
            }
            if (Exercice.exerciseTypologie == null)
            {
                Value = true;
                return;
            }
            if (Exercice.lood == 0)
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

            var exercice = new Exercice
            {
                id = Exercice.id,
                level = SelectedLevel.Key,
                name = Exercice.name,
                exerciseTypologie = Exercice.exerciseTypologie,
                totalSeries = Exercice.totalSeries,
                totalReputation = Exercice.totalReputation,
                breakDuration = Exercice.breakDuration,
                speed = Exercice.speed,
                concentric = Exercice.concentric,
                lood = Exercice.lood,
                secondLood = Exercice.secondLood,
                eccentric = Exercice.eccentric,
                loodEccentric = Exercice.loodEccentric,
                secondLoodEccentric = Exercice.secondLoodEccentric
            };
            var response = await apiService.Put<Exercice>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/exercice",
                 accesstoken,
                  exercice);
             if (!response.IsSuccess)
             {
                 await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                 return;
             }

            Value = false;
            ExerciceViewModel.GetInstance().Update(exercice);

            DependencyService.Get<INotification>().CreateNotification("GYM TECH", "Exercice Updated");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
        #endregion

        #region Commands
        public ICommand UpdateExercice
        {
            get
            {
                return new Command(() =>
                {
                    EditExercice();
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
        public Command SeriesExercice
        {
            get
            {
                return new Command(() =>
                {
                    PopupNavigation.Instance.PushAsync(new SeriesExercicePage(Exercice));

                });
            }
        }
        public Command ImagesExercice
        {
            get
            {
                return new Command(() =>
                {
                    PopupNavigation.Instance.PushAsync(new ImagesExercicePage(Exercice));

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
