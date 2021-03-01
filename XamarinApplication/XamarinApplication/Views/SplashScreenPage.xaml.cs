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
    public partial class SplashScreenPage : ContentPage
    {
        public SplashScreenPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await stackLayout.ScaleTo(1, 1500);
            await stackLayout.ScaleTo(0.8, 1500, Easing.Linear);
            await stackLayout.ScaleTo(150, 1200, Easing.Linear);
            Application.Current.MainPage = new NavigationPage(new LoginPage());
            //Application.Current.MainPage.Navigation.PopModalAsync(new MainPage());
            //await Navigation.PushModalAsync(new MainPage());
        }
    }
}