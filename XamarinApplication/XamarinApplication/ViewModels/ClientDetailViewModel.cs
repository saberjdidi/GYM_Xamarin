using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XamarinApplication.Models;

namespace XamarinApplication.ViewModels
{
    public class ClientDetailViewModel
    {
        public INavigation Navigation { get; set; }
        public ClientDetailViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
        }
        public User User { get; set; }
    }
}
