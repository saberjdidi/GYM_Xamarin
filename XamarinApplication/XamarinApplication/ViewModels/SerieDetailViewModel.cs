using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XamarinApplication.Models;

namespace XamarinApplication.ViewModels
{
    public class SerieDetailViewModel
    {
        public INavigation Navigation { get; set; }
        public SerieDetailViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
        }
        public Serie Serie { get; set; }
    }
}
