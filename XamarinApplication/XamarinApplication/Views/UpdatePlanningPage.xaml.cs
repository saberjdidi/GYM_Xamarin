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
    public partial class UpdatePlanningPage : PopupPage
    {
        public UpdatePlanningPage(Planning planning)
        {
            InitializeComponent();

            var updatePlanningViewModel = new UpdatePlanningViewModel(Navigation);
            updatePlanningViewModel.Planning = planning;
            BindingContext = updatePlanningViewModel;
        }
    }
}