using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinApplication.Views
{
    public class SplashScreen : ContentPage
    {
        Image SplashImage;
        public SplashScreen()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            var container = new AbsoluteLayout();
            SplashImage = new Image
            {
                Source = "logo.png"
            };
            AbsoluteLayout.SetLayoutFlags(SplashImage, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(SplashImage, new Rectangle(1, 1, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            container.Children.Add(SplashImage);
            this.Content = container;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await SplashImage.ScaleTo(1, 1500);
            await SplashImage.ScaleTo(0.8, 1500, Easing.Linear);
            await SplashImage.ScaleTo(150, 1200, Easing.Linear);
            Application.Current.MainPage = new NavigationPage(new LoginPage());
            //Application.Current.MainPage.Navigation.PopModalAsync(new MainPage());
            //await Navigation.PushModalAsync(new MainPage());
        }
    }
}
