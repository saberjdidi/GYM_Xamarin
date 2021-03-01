using Rg.Plugins.Popup.Pages;
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
    public partial class BiaClientPage : PopupPage
    {
        public BiaClientPage(User user)
        {
            InitializeComponent();
            var biaViewModel = new BiaClientViewModel();
            biaViewModel.User = user;
            BindingContext = biaViewModel;
        }

        private async void Close_Bia(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
        private async void Update_Bia(object sender, EventArgs e)
        {
            TappedEventArgs tappedEventArgs = (TappedEventArgs)e;
            BiaSportif bia = ((BiaClientViewModel)BindingContext).Bia.Where(ser => ser.id == (int)tappedEventArgs.Parameter).FirstOrDefault();
            await PopupNavigation.Instance.PushAsync(new UpdateBiaClientPage(bia));
        }
    }
}