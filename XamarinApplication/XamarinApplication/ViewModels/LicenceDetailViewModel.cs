using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XamarinApplication.Models;

namespace XamarinApplication.ViewModels
{
    public class LicenceDetailViewModel
    {
        public INavigation Navigation { get; set; }
        public LicenceDetailViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
        }
        public Licence Licence { get; set; }
    }
}
