using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApplication.Helpers;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel(Navigation);

            NavigationPage.SetHasNavigationBar(this, false);  // Hide nav bar

            //Password.Placeholder = Languages.Password; 
        }

        //close application with button Back
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();

            return false;
        }

        private void ShowPassword_Tapped(object sender, EventArgs e)
        {
            if (PasswordEntry.IsPassword == true)
            {
                PasswordEntry.IsPassword = false;
               // passwordEye.Source = "visibility";
            }
            else
            {
                PasswordEntry.IsPassword = true;
               // passwordEye.Source = "visibility";
            }
        }
    }
}