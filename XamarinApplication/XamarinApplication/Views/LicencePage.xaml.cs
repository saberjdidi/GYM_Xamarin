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
    public partial class LicencePage : ContentPage
    {
        public LicencePage()
        {
            InitializeComponent();
        }

        /*protected override void OnAppearing()
        {
            (this.BindingContext as LicenceViewModel).LoadLicence();
        }*/

        private async void Licence_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var licence = e.Item as Licence;
            await Navigation.PushAsync(new LicenceDetailPage(licence));
        }
        private async void Licence_Detail(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var licence = mi.CommandParameter as Licence;
            // await PopupNavigation.Instance.PushAsync(new GimDetailPage(gim));
            await Navigation.PushAsync(new LicenceDetailPage(licence));
        }
        private async void Update_Licence(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var licence = mi.CommandParameter as Licence;
            await PopupNavigation.Instance.PushAsync(new UpdateLicencePage(licence));
        }
        private async void Add_Licence(object sender, EventArgs e)
        {
            // await Navigation.PushAsync(new AddGimPage());
            await PopupNavigation.Instance.PushAsync(new AddLicencePage());
        }
    }
}