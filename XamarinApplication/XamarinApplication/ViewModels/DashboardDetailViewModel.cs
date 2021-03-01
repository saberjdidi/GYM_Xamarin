using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XamarinApplication.Models;

namespace XamarinApplication.ViewModels
{
    public class DashboardDetailViewModel
    {
        public INavigation Navigation { get; set; }
        public DashboardDetailViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
        }
        public Machine Machine { get; set; }
    }
}
