using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfigurationMachinePage : ContentPage
    {
        public ConfigurationMachinePage()
        {
            InitializeComponent();

            //NavigationPage.SetHasNavigationBar(this, false);  // Hide nav bar   
        }

        private async void Next_Page(object sender, EventArgs e)
        {
            //await Navigation.PushModalAsync(new SecondPage());
            await PopupNavigation.Instance.PushAsync(new ConfigurationMachinePopupPage());
        }
    }
}