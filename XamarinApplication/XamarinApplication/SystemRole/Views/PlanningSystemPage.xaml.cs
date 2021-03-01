using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApplication.SystemRole.ViewModels;

namespace XamarinApplication.SystemRole.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlanningSystemPage : ContentPage
    {
        public PlanningSystemPage()
        {
            InitializeComponent();
            BindingContext = new PlanningSystemViewModel();
        }
    }
}