using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProgramPage : ContentPage
    {
        public User User { get; set; }
        public ProgramPage()
        {
            InitializeComponent();
        }

         /*protected override void OnAppearing()
         {
             (this.BindingContext as ProgramViewModel).LoadProgram();
         }*/

        private async void Program_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var program = e.Item as Program;
            await Navigation.PushAsync(new ProgramDetailPage(program));
        }
        private async void Program_Detail(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var program = mi.CommandParameter as Program;
            await Navigation.PushAsync(new ProgramDetailPage(program));
        }
        private async void Update_Program(object sender, EventArgs e)
        {
            //Current User
            var accesstoken = Settings.AccessToken;
            var username = Settings.Username;
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            var uri = "http://phoneofficine.it/niini-gim/user/username?username=" + username;
            var result = client.GetStringAsync(uri);
            User = JsonConvert.DeserializeObject<User>(result.Result);

            if (User.role.name.Equals("ROLE_ADMIN"))
            {
                var mi = ((MenuItem)sender);
                var program = mi.CommandParameter as Program;
                await PopupNavigation.Instance.PushAsync(new UpdateProgramPage(program));
            }
            else
            {
              await DisplayAlert("Exit", "You Don't Have Permission", "OK");
            }
        }

        private async void Add_Program(object sender, EventArgs e)
        {
            // await Navigation.PushAsync(new AddGimPage());
            await PopupNavigation.Instance.PushAsync(new AddProgramPage());
        }
    }
}