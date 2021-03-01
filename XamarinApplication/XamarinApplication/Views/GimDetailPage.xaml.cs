using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GimDetailPage : TabbedPage
    {
        public GimDetailPage(Gim gim)
        {
            InitializeComponent();

            var gimViewModel = new GimDetailViewModel(Navigation);
            gimViewModel.Gim = gim;
            BindingContext = gimViewModel;

            InformationsTitle.Title = Languages.Informations;
            LicenceTitle.Title = Languages.Licence;
        }
    }
}