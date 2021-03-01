using Rg.Plugins.Popup.Services;
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
    public partial class PlanningDetailPage : TabbedPage
    {
        public PlanningDetailPage(Planning planning)
        {
            InitializeComponent();

            var planningViewModel = new PlanningDetailViewModel(Navigation);
            planningViewModel.Planning = planning;
            BindingContext = planningViewModel;

            ProgramsTitle.Title = Languages.Programs;
            SportifTitle.Title = Languages.Sportif;
            //InformationsTitle.Title = Languages.Informations;
            //ExercicesTitle.Title = Languages.Exercices;
        }

        private async void Program_Exercices(object sender, ItemTappedEventArgs e)
        {
            var program = e.Item as Program;
            await PopupNavigation.Instance.PushAsync(new PlannigPrograms_DetailPage(program));
        }
    }
}