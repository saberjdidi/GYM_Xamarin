using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
    public class RegisterViewModel
    {
        private ApiServices _apiServices = new ApiServices();
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Message { get; set; }

        public ICommand RegisterCommand
        {
            get
            {
                return new Command(async() => 
                {
                   var isSuccess = await _apiServices.RegisterAsync(Username, Password, ConfirmPassword);

                    Settings.Username = Username;
                    Settings.Password = Password;

                    if (isSuccess)
                        Message = "Registered Successfully";
                    else
                        Message = "Retry Later";
                    
                });
            }
        }
    }
}
