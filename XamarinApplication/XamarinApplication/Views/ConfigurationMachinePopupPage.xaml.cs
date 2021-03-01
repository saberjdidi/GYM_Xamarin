using Rg.Plugins.Popup.Pages;
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
    public partial class ConfigurationMachinePopupPage : PopupPage
    {
        public ConfigurationMachinePopupPage()
        {
            InitializeComponent();
        }

        private async void Previous_Page(object sender, EventArgs e)
        {
            //await Navigation.PushModalAsync(new MainPage());
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}