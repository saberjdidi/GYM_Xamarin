using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApplication.Sportif.ViewModels;

namespace XamarinApplication.Sportif.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlanningSportifPage : ContentPage
    {
        public PlanningSportifPage()
        {
            InitializeComponent();
            BindingContext = new PlanningSportifViewModel();
        }
    }
}