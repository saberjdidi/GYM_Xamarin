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
    public partial class ParametresClientPage : PopupPage
    {
        public ParametresClientPage(User user)
        {
            InitializeComponent();

            var parametresViewModel = new ParametresClientViewModel();
            parametresViewModel.User = user;
            BindingContext = parametresViewModel;
        }

        private async void Close_Parametres(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
        private async void Update_Parameter(object sender, EventArgs e)
        {
            TappedEventArgs tappedEventArgs = (TappedEventArgs)e;
            ParametreSportif parametre = ((ParametresClientViewModel)BindingContext).Parametres.Where(ser => ser.id == (int)tappedEventArgs.Parameter).FirstOrDefault();
            await PopupNavigation.Instance.PushAsync(new UpdateParameterClientPage(parametre));
        }
        private async void Parameter_Details(object sender, ItemTappedEventArgs e)
        {
            var parametre = e.Item as ParametreSportif;
            await PopupNavigation.Instance.PushAsync(new ParameterClientDetailPage(parametre));
        }
    }
}