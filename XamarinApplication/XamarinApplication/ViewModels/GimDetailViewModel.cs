using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XamarinApplication.Models;

namespace XamarinApplication.ViewModels
{
    public class GimDetailViewModel
    {
        public INavigation Navigation { get; set; }
        public GimDetailViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
        }
        public Gim Gim { get; set; }
    }
}
