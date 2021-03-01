using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApplication.Entreneur.ViewModels;
using XamarinApplication.Models;
using XamarinApplication.Views;

namespace XamarinApplication.Entreneur.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MachineGYMPage : ContentPage
    {
        public MachineGYMPage()
        {
            InitializeComponent();
            BindingContext = new MachineGYMViewModel();
        }

        private async void Machine_Details(object sender, ItemTappedEventArgs e)
        {
            var machine = e.Item as Machine;
            await Navigation.PushAsync(new MachineDetailPage(machine));
        }
    }
}