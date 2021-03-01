using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XamarinApplication.Models;

namespace XamarinApplication.ViewModels
{
    public class PurchasedPackageDetailViewModel
    {
        public INavigation Navigation { get; set; }
        public PurchasedPackageDetailViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
        }
        public PurchasedPackage PurchasedPackage { get; set; }
    }
}
