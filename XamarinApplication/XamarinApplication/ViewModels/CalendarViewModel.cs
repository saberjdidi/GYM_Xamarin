using Newtonsoft.Json;
using Syncfusion.SfCalendar.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
    public class CalendarViewModel : INotifyPropertyChanged
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
        public CalendarViewModel()
        {
            restService = new RestService();
            apiService = new ApiServices();
            GetPlannings();

        }
        #endregion

        #region Methods
        /* public async void GetPlannings()
         {
             var connection = await restService.CheckConnection();
             var accesstoken = Settings.AccessToken;

             if (!connection.IsSuccess)
             {
                 await Application.Current.MainPage.DisplayAlert(
                     "Error",
                     connection.Message,
                     "Ok");
                 await Application.Current.MainPage.Navigation.PopAsync();
                 return;
             }
             var response = await restService.GetList<PlanningCalendar>(
                  "http://phoneofficine.it",
                  "/niini-gim",
                  "/planning",
                  accesstoken);
             if (!response.IsSuccess)
             {
                 await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                 return;
             }
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
                     //StartTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddMilliseconds(eventsList[i].startsAt),
                     //EndTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddMilliseconds(eventsList[i].endsAt),
                     //Color = Color.FromHex(eventsList[i].color.primary)
                 });
             }

         }*/
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
            SfCalendar calendar = new SfCalendar();
            
            DateTime _date = DateTime.Now;
            DateTime date = new DateTime();
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var _firstdateString = firstDayOfMonth.ToString("dd/MM/yyyy");
            Debug.WriteLine("********_firstdateString**********");
            Debug.WriteLine(_firstdateString);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var _lastdateString = lastDayOfMonth.ToString("dd/MM/yyyy");
            Debug.WriteLine("********_lastdateString**********");
            Debug.WriteLine(_lastdateString);


            if (User.role.name.Equals("ROLE_ADMIN"))
            {
                var _searchRequest = new SearchRequest
                {
                };
                var response = await restService.PostData<PlanningCalendar>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/planning/search?sortedBy=date&order=asc", //&from="+ _firstdateString + "&to="+ _lastdateString,
                 accesstoken,
                 _searchRequest);
                if (!response.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                    return;
                }
                eventsList = (List<PlanningCalendar>)response.Result;
                /*if (eventsList.Count == 0)
                {
                    return;
                }*/
            }
            if (User.role.name.Equals("ROLE_SPORTIF"))
            {
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
            }
            if (User.role.name.Equals("ROLE_ENTRENEUR"))
            {
                var _coach = new SearchByCoach()
                {
                    coach = User
                };
                var _sportif = new SearchBySportif
                {
                    sportif = _coach
                };

                var response = await apiService.PlanningData<PlanningCalendar>(
                     "http://phoneofficine.it",
                     "/niini-gim",
                     "/planning/searchPagination?sortedBy=date&order=asc&maxResult=100",
                     accesstoken,
                     _sportif);
                eventsList = (List<PlanningCalendar>)response.Result;
            }
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
                        //StartTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddMilliseconds(eventsList[i].startsAt),
                        //EndTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddMilliseconds(eventsList[i].endsAt),
                        //Color = Color.FromHex(eventsList[i].color.primary)
                    });
                }
            /* for (int i = 0; i < eventsList.Count; i++)
            {
            if (CalendarInlineEvents.Count == 0)
            {
                //await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;

            } else
            {
                CalendarInlineEvents.Add(new CalendarInlineEvent()
                {
                    Subject = eventsList[i].program.name,
                    StartTime = eventsList[i].date,
                    EndTime = eventsList[i].date,
                    Color = Color.FromHex("#8B2635")
                });
            }
            }*/

        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region EventStatic
        // Create events  //use in constructor
        /*
        CalendarInlineEvent event1 = new CalendarInlineEvent()
        {
            StartTime = DateTime.Today.AddHours(9),
            EndTime = DateTime.Today.AddHours(10),
            Subject = "Meeting",
            Color = Color.Green
        };
        CalendarInlineEvents.Add(event1);
            */
        #endregion
    }
}
