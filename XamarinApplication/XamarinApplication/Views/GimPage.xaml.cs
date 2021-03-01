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
    public partial class GimPage : ContentPage
    {
        public GimPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            (this.BindingContext as GimViewModel).LoadGim();
        }

        private async void Gim_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var gim = e.Item as Gim;
            await Navigation.PushAsync(new GimDetailPage(gim));
        }
        private async void GYM_Detail(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var gim = mi.CommandParameter as Gim;
            // await PopupNavigation.Instance.PushAsync(new GimDetailPage(gim));
            await Navigation.PushAsync(new GimDetailPage(gim));
        }
        private async void Update_GYM(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var gim = mi.CommandParameter as Gim;
            await PopupNavigation.Instance.PushAsync(new UpdateGYMPage(gim));
        }

        private async void Add_Gim(object sender, EventArgs e)
        {
           // await Navigation.PushAsync(new AddGimPage());
            await PopupNavigation.Instance.PushAsync(new AddGimPage());
        }
    }
}