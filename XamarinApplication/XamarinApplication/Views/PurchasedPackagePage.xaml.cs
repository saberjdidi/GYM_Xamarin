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
    public partial class PurchasedPackagePage : ContentPage
    {
        public PurchasedPackagePage()
        {
            InitializeComponent();
            BindingContext = new PurchasedPackageViewModel();
        }

        private async void Purchased_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var purchasedPackage = e.Item as PurchasedPackage;
            await Navigation.PushAsync(new PurchasedPackageDetailPage(purchasedPackage));
        }
    }
}