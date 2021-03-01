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
    public partial class ExerciceDetailPage : TabbedPage
    {
        public ExerciceDetailPage(Exercice exercice)
        {
            InitializeComponent();

            var exerciceViewModel = new ExerciceDetailViewModel(Navigation);
            exerciceViewModel.Exercice = exercice;
            BindingContext = exerciceViewModel;

            InformationsTitle.Title = Languages.Informations;
            CoachTitle.Title = Languages.Coach;
        }
    }
}