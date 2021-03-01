using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class PlanningPage : ContentPage
    {
        public PlanningPage()
        {
            InitializeComponent();
            BindingContext = new PlanningViewModel();
           
        }


        protected override void OnAppearing()
        {
            (this.BindingContext as PlanningViewModel).GetPlannings();
        }

        private async void Planning_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var planning = e.Item as Planning;
            await Navigation.PushAsync(new PlanningDetailPage(planning));
        }
        private async void Planning_Detail(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var planning = mi.CommandParameter as Planning;
            await Navigation.PushAsync(new PlanningDetailPage(planning));
        }
        private async void Update_Planning(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var planning = mi.CommandParameter as Planning;
            await PopupNavigation.Instance.PushAsync(new UpdatePlanningPage(planning));
        }

        private async void Add_Planning(object sender, EventArgs e)
        {
            // await Navigation.PushAsync(new AddGimPage());
            await PopupNavigation.Instance.PushAsync(new AddPlanningPage());
        }
    }
}