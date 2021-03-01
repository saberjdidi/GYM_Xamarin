using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XamarinApplication.Models;

namespace XamarinApplication.ViewModels
{
    public class ParameterClientDetailViewModel
    {
        public INavigation Navigation { get; set; }
        public ParameterClientDetailViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
        }
        public ParametreSportif Parameter { get; set; }
    }
}
