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
    public partial class DashboardDetailPage : TabbedPage
    {
        public DashboardDetailPage(Machine machine)
        {
            InitializeComponent();

            var dashboardViewModel = new DashboardDetailViewModel(Navigation);
            dashboardViewModel.Machine = machine;
            BindingContext = dashboardViewModel;
        }
    }
}