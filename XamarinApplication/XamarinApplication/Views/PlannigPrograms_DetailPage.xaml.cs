using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
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
    public partial class PlannigPrograms_DetailPage : PopupPage
    {
        public PlannigPrograms_DetailPage(Program program)
        {
            InitializeComponent();

            var exercicesProgram = new PlanningProgramsDetailViewModel(Navigation);
            exercicesProgram.Program = program;
            BindingContext = exercicesProgram;
        }

        private async void Close_Popup(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }

        private async void Close_Exercices(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}