using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApplication.Models;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PurchasedPackageDetailPage : TabbedPage
    {
        public PurchasedPackageDetailPage(PurchasedPackage purchasedPackage)
        {
            InitializeComponent();

            var purchasedViewModel = new PurchasedPackageDetailViewModel(Navigation);
            purchasedViewModel.PurchasedPackage = purchasedPackage;
            BindingContext = purchasedViewModel;
        }
    }
}