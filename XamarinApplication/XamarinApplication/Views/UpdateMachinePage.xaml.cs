using Rg.Plugins.Popup.Pages;
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
    public partial class UpdateMachinePage : PopupPage
    {
        public UpdateMachinePage(Machine machine)
        {
            InitializeComponent();

            var updateMachineViewModel = new UpdateMachineViewModel(Navigation);
            updateMachineViewModel.Machine = machine;
            BindingContext = updateMachineViewModel;
        }
    }
}