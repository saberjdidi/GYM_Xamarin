using Newtonsoft.Json;
using Syncfusion.SfCalendar.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.Services;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Sportif.ViewModels
{
    public class CalendarClientViewModel : BaseViewModel
    {
        #region Services
        private RestService restService;
        private ApiServices apiService;
        #endregion

        #region Attributes
        private ObservableCollection<PlanningCalendar> eventCollection;
        private List<PlanningCalendar> eventsList;
        #endregion

        #region Properties
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
        public ObservableCollection<PlanningCalendar> EventCollection
        {
            get { return eventCollection; }
            set
            {
                eventCollection = value;
                OnPropertyChanged();
            }
        }
        private PlanningCalendar _planningCalendar;
        public PlanningCalendar PlanningCalendar
        {
            get { return _planningCalendar; }
            set
            {
                _planningCalendar = value;
                OnPropertyChanged();
            }
        }
        public string _programs = "";
        public string programss
        {
            get { return _programs; }
            set
            {
                _programs = value;
                OnPropertyChanged();
            }
        }
        public CalendarEventCollection CalendarInlineEvents { get; set; } = new CalendarEventCollection();
        #endregion

        #region Constructors
        public CalendarClientViewModel()
        {
            restService = new RestService();
            apiService = new ApiServices();
            GetPlannings();

        }
        #endregion

        #region Methods
        public async void GetPlannings()
        {
            //Current User
            var accesstoken = Settings.AccessToken;
            var username = Settings.Username;
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            var uri = "http://phoneofficine.it/niini-gim/user/username?username=" + username;
            var result = client.GetStringAsync(uri);
            User = JsonConvert.DeserializeObject<User>(result.Result);
            Debug.WriteLine("********User Menu********");
            Debug.WriteLine(User);
            //Connection Internet
            var connection = await restService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Ok");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }
            //Planning method 
            var _sportif = new ParametreClient
            {
                sportif = User
            };

            var response = await apiService.ParametresData<PlanningCalendar>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/planning/searchPagination?sortedBy=date&order=asc&maxResult=100",
                 accesstoken,
                 _sportif);
            eventsList = (List<PlanningCalendar>)response.Result;
            if (eventsList != null)

                for (int i = 0; i < eventsList.Count; i++)
                {

                    if (eventsList[i].programs != null)
                    {
                        programss = "";
                        eventsList[i].programs.ForEach(
                                  prog =>
                                  {
                                      programss = programss + prog.name + ", ";
                                  });
                    }
                    CalendarInlineEvents.Add(new CalendarInlineEvent()
                    {
                        Subject = "Athletic: " + eventsList[i].sportif.username + " " + eventsList[i].sportif.lastname + "\n /Programs: " + programss,
                        StartTime = eventsList[i].date,
                        EndTime = eventsList[i].date,
                        Color = Color.FromHex("#8B2635")
                    });
                }
        }
    
        #endregion
    }
}
