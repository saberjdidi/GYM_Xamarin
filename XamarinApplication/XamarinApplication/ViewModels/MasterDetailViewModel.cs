using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Timers;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Entreneur.Views;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.Resources;
using XamarinApplication.Services;
using XamarinApplication.Sportif.Views;
using XamarinApplication.SystemRole.Views;
using XamarinApplication.Views;

namespace XamarinApplication.ViewModels
{
    public class MasterDetailViewModel : INotifyPropertyChanged
    {
        #region Services
        private ApiServices apiService;
        #endregion
        //i18n Collection Name 
        public string Collection { get; set; } = Languages.Collection;

        #region Attributes
        public INavigation Navigation { get; set; }
        private ObservableCollection<MasterMenu> _menuItems;
        private ObservableCollection<Carousel> _carousel;
        #endregion

        // public string Username = Settings.Username;

        // public List<Carousel> CarouselImage { get => GetCarousels(); }
        #region Constructor
        public MasterDetailViewModel(INavigation _navigation)
        {
            apiService = new ApiServices();
            Navigation = _navigation;
            PopulateMenu();
            GetCarousels();
            GetCollections();
            GetCurrentUser();

        }
        #endregion

        #region Properties
        public ObservableCollection<MasterMenu> MenuItems
        {
            get
            {
                return _menuItems;
            }
            set
            {
                if (value != null)
                {
                    _menuItems = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<Carousel> CarouselImage
        {
            get
            {
                return _carousel;
            }
            set
            {
                _carousel = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Carousel> CollectionList
        {
            get
            {
                return _carousel;
            }
            set
            {
                _carousel = value;
                OnPropertyChanged();
            }
        }

        MasterMenu _selectedMenu;
        public MasterMenu SelectedMenu
        {
            get
            {
                return _selectedMenu;
            }
            set
            {
                if (_selectedMenu != null)
                {
                    _selectedMenu.Selected = false;
                    _selectedMenu.MenuIcon = _selectedMenu.MenuIcon.Substring(0, _selectedMenu.MenuIcon.Length - 6);
                }


                _selectedMenu = value;

                if (_selectedMenu != null)
                {
                    _selectedMenu.Selected = true;
                    _selectedMenu.MenuIcon += "_color";
                    MessagingCenter.Send<MasterMenu>(_selectedMenu, "OpenMenu");
                }
            }
        }

        private User _user = null;
        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Methods
        public async void GetCurrentUser()
        {
            using (var client = new HttpClient())
            {
                var accesstoken = Settings.AccessToken;
                var username = Settings.Username;
                var connection = await apiService.CheckConnection();

                if (!connection.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        connection.Message,
                        "Ok");
                    await Application.Current.MainPage.Navigation.PopAsync();
                    return;
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
                var uri = "http://phoneofficine.it/niini-gim/user/username?username="+username;
                var result = await client.GetStringAsync(uri);
                Settings.CurrentUser = result;
                Debug.WriteLine("********Settings.CurrentUser********");
                Debug.WriteLine(Settings.CurrentUser);
                User = JsonConvert.DeserializeObject<User>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                
            }
        }
        public void PopulateMenu()
        {
            MenuItems = new ObservableCollection<MasterMenu>();
            //get current user
            var accesstoken = Settings.AccessToken;
            var username = Settings.Username;
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            var uri = "http://phoneofficine.it/niini-gim/user/username?username=" + username;
            var result = client.GetStringAsync(uri);
            User = JsonConvert.DeserializeObject<User>(result.Result);
            Debug.WriteLine("********User Menu********");
            Debug.WriteLine(User);
            //menu
           if(User.role.name.Equals("ROLE_ADMIN"))
            { 
            MenuItems.Add(new MasterMenu { MenuName = "Dashboard", MenuIcon = "dashboard", TargetType = typeof(DashboardPage) });
            MenuItems.Add(new MasterMenu { MenuName = Languages.Planning, MenuIcon = "planning", TargetType = typeof(PlanningPage) });
            MenuItems.Add(new MasterMenu { MenuName = Languages.Calendar, MenuIcon = "calendar", TargetType = typeof(CalendarPage) });
            MenuItems.Add(new MasterMenu { MenuName = Languages.GYM, MenuIcon = "gym", TargetType = typeof(GimPage) });
            MenuItems.Add(new MasterMenu { MenuName = Languages.Licence, MenuIcon = "licence", TargetType = typeof(LicencePage) });
            MenuItems.Add(new MasterMenu { MenuName = Languages.Machine, MenuIcon = "machine", TargetType = typeof(MachinePage) });
            MenuItems.Add(new MasterMenu { MenuName = Languages.Exercice, MenuIcon = "exercice", TargetType = typeof(ExercicePage) });
            MenuItems.Add(new MasterMenu { MenuName = Languages.Program, MenuIcon = "program", TargetType = typeof(ProgramPage) });
            MenuItems.Add(new MasterMenu { MenuName = "Client", MenuIcon = "person", TargetType = typeof(ClientPage) });
            MenuItems.Add(new MasterMenu { MenuName = Languages.Configuration, MenuIcon = "settings", TargetType = typeof(ConfigurationMachinePage) });
                //MenuItems.Add(new MasterMenu { MenuName = Languages.Serie, MenuIcon = "serie", TargetType = typeof(SeriePage) });
                // MenuItems.Add(new MasterMenu { MenuName = Languages.PurchasedPackage, MenuIcon = "purchase", TargetType = typeof(PurchasedPackagePage) });
                // MenuItems.Add(new MasterMenu { MenuName = Languages.ConsumedPackage, MenuIcon = "consumed", TargetType = typeof(ConsumedPackagePage) });
            }
            //if (currentUser == "ROLE_SPORTIF")
            if (User.role.name.Equals("ROLE_SPORTIF"))
            {
               //MenuItems.Add(new MasterMenu { MenuName = "Dashboard", MenuIcon = "dashboard", TargetType = typeof(DashboardPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.Planning, MenuIcon = "planning", TargetType = typeof(PlanningSportifPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.Calendar, MenuIcon = "calendar", TargetType = typeof(CalendarPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.Configuration, MenuIcon = "settings", TargetType = typeof(ConfigurationMachinePage) });
            }
            if (User.role.name.Equals("ROLE_ENTRENEUR"))
            {
                MenuItems.Add(new MasterMenu { MenuName = "Dashboard", MenuIcon = "dashboard", TargetType = typeof(DashboardPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.Planning, MenuIcon = "planning", TargetType = typeof(PlanningEntreneurPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.Calendar, MenuIcon = "calendar", TargetType = typeof(CalendarPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.GYM, MenuIcon = "gym", TargetType = typeof(GYMEntreneurPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.Machine, MenuIcon = "machine", TargetType = typeof(MachineGYMPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.Exercice, MenuIcon = "exercice", TargetType = typeof(ExerciceEntreneurPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.Program, MenuIcon = "program", TargetType = typeof(ProgramPage) });
                MenuItems.Add(new MasterMenu { MenuName = "Client", MenuIcon = "person", TargetType = typeof(ClientPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.Configuration, MenuIcon = "settings", TargetType = typeof(ConfigurationMachinePage) });
            }
            if (User.role.name.Equals("ROLE_SYSTEM"))
            {
                MenuItems.Add(new MasterMenu { MenuName = Languages.Planning, MenuIcon = "planning", TargetType = typeof(PlanningSystemPage) });
            }

            }
        public void GetCarousels()
        {
            CarouselImage = new ObservableCollection<Carousel>();
            CarouselImage.Add(new Carousel { Heading = "Xamarin GYM", Message = "Fitness GYM", Caption = Languages.BestFitness, Image = "https://p7.hiclipart.com/preview/95/773/163/weight-training-muscle-bodybuilding-barbell-lean-body-mass-chest-muscle.jpg" });
            CarouselImage.Add(new Carousel { Heading = "Xamarin GYM", Message = Languages.Exercice, Caption = Languages.BestFitness, Image = "https://p7.hiclipart.com/preview/686/358/489/fitness-centre-human-body-strength-training-physical-exercise-gym.jpg" });
            CarouselImage.Add(new Carousel { Heading = "Xamarin GYM", Message = Languages.Machine, Caption = Languages.BestFitness, Image = "https://p7.hiclipart.com/preview/204/268/966/fitness-centre-exercise-equipment-exercise-machine-physical-fitness-gym.jpg" });
            CarouselImage.Add(new Carousel { Heading = "Xamarin GYM", Message = Languages.Planning, Caption = Languages.BestFitness, Image = "https://p7.hiclipart.com/preview/485/851/790/zac-smith-mobile-phones-coach-physical-fitness-personal-trainer-fitness-app.jpg" });
            CarouselImage.Add(new Carousel { Heading = "Xamarin GYM", Message = Languages.Program, Caption = Languages.BestFitness, Image = "https://p7.hiclipart.com/preview/964/61/92/computer-programming-method-periodization-gym-muscle-building-poster.jpg" });

        }
        public void GetCollections()
        {
            CollectionList = new ObservableCollection<Carousel>();
            CollectionList.Add(new Carousel { Message = "Best Exercice", Image = "https://p7.hiclipart.com/preview/489/394/355/weight-training-muscle-bodybuilding-olympic-weightlifting-crossfit-dumbbell-exercise.jpg" });
            CollectionList.Add(new Carousel { Message = "Machine", Image = "https://p7.hiclipart.com/preview/701/296/871/exercise-equipment-exercise-machine-fitness-centre-dumbbell-physical-exercise-barbell.jpg" });
            CollectionList.Add(new Carousel { Message = "Machine", Image = "https://p7.hiclipart.com/preview/371/420/880/dumbbell-icon-dumbbells-png.jpg" });
            CollectionList.Add(new Carousel { Message = "Bicepse Exercice", Image = "https://p7.hiclipart.com/preview/925/566/485/exercise-fitness-centre-personal-trainer-physical-fitness-general-fitness-training-dumbbell.jpg" });
            CollectionList.Add(new Carousel { Message = "Fitness GYM", Image = "https://p7.hiclipart.com/preview/402/515/819/physical-fitness-fitness-centre-exercise-personal-trainer-general-fitness-training-gym-class.jpg" });
            CollectionList.Add(new Carousel { Message = "Planning GYM", Image = "https://p7.hiclipart.com/preview/964/115/737/physical-fitness-personal-trainer-coach-fitness-professional-fitness-centre-personal-training.jpg" });

        }
        #endregion

        /*public ICommand LogoutCommand
        {
            get
            {
                return new Command(async() =>
                {
                    
                    var confirm = Application.Current.MainPage.DisplayAlert("Exit", "Do you wan't to exit the App ?", "Yes", "No");
                    if (confirm.Equals("Yes"))
                    {
                        Settings.AccessToken = string.Empty;
                        Debug.WriteLine(Settings.Username);
                        Settings.Username = string.Empty;
                        Debug.WriteLine(Settings.Password);
                        Settings.Password = string.Empty;

                        await Navigation.PushModalAsync(new LoginPage());
                    }
                    else if(confirm.Equals("No"))
                    {
                        await Navigation.PushModalAsync(new MainPage());
                    }
                });
            }
        }*/
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
