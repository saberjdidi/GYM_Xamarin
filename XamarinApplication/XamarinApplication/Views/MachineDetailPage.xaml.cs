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
    public partial class MachineDetailPage : TabbedPage
    {
        public MachineDetailPage(Machine machine)
        {
            InitializeComponent();

            var machineViewModel = new MachineDetailViewModel(Navigation);
            machineViewModel.Machine = machine;
            BindingContext = machineViewModel;

            InformationsTitle.Title = Languages.Informations;
            LicenceTitle.Title = Languages.Licence;
        }
    }
}