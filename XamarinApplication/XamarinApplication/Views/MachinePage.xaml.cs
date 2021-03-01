using Rg.Plugins.Popup.Services;
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
    public partial class MachinePage : ContentPage
    {
        public MachinePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            (this.BindingContext as MachineViewModel).LoadMachine();
        }

        private async void Machine_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var machine = e.Item as Machine;
            await Navigation.PushAsync(new MachineDetailPage(machine));
        }
        private async void Machine_Detail(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var machine = mi.CommandParameter as Machine;
            await Navigation.PushAsync(new MachineDetailPage(machine));
        }
        private async void Update_Machine(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var machine = mi.CommandParameter as Machine;
            await PopupNavigation.Instance.PushAsync(new UpdateMachinePage(machine));
        }

        private async void Add_Machine(object sender, EventArgs e)
        {
            // await Navigation.PushAsync(new AddGimPage());
            await PopupNavigation.Instance.PushAsync(new AddMachinePage());
        }
    }
}