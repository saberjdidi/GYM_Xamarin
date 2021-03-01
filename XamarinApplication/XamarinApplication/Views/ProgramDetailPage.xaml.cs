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
    public partial class ProgramDetailPage : TabbedPage
    {
        public ProgramDetailPage(Program program)
        {
            InitializeComponent();

            var programViewModel = new ProgramDetailViewModel(Navigation);
            programViewModel.Program = program;
            BindingContext = programViewModel;

            MachineTitle.Title = Languages.Machines;
            ExercicesTitle.Title = Languages.Exercices;
            CoachTitle.Title = Languages.Coach;
        }
    }
}