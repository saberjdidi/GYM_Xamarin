using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApplication.Models;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashboardPage : ContentPage
    {
        public DashboardPage()
        {
            InitializeComponent();

            BindingContext = new DashboardViewModel();
        }

        private async void Dashboard_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var machine = e.Item as Machine;
            await Navigation.PushAsync(new DashboardDetailPage(machine));
        }
    }
}