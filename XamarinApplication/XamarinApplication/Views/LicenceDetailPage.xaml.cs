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
    public partial class LicenceDetailPage : TabbedPage
    {
        public LicenceDetailPage(Licence licence)
        {
            InitializeComponent();

            var licenceViewModel = new LicenceDetailViewModel(Navigation);
            licenceViewModel.Licence = licence;
            BindingContext = licenceViewModel;

            InformationsTitle.Title = Languages.Informations;
        }
    }
}