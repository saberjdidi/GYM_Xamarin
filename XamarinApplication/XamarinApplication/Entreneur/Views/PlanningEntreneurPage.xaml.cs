using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApplication.Entreneur.ViewModels;
using XamarinApplication.Models;
using XamarinApplication.Views;

namespace XamarinApplication.Entreneur.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlanningEntreneurPage : ContentPage
    {
        public PlanningEntreneurPage()
        {
            InitializeComponent();
            BindingContext = new PlanningEntreneurViewModel();
        }
        private async void Add_Planning(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new AddPlanningPage());
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
    }
}