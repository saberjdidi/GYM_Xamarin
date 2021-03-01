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
    public partial class UpdateExercicePage : PopupPage
    {
        public UpdateExercicePage(Exercice exercice)
        {
            InitializeComponent();

            var updateExerciceViewModel = new UpdateExerciceViewModel(Navigation);
            updateExerciceViewModel.Exercice = exercice;
            BindingContext = updateExerciceViewModel;
        }
    }
}