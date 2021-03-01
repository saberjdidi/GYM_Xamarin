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
    public partial class ImagesExercicePage : PopupPage
    {
        public ImagesExercicePage(Exercice exercice)
        {
            InitializeComponent();

            var imagesExerciceViewModel = new ImagesExerciceViewModel();
            imagesExerciceViewModel.Exercice = exercice;
            BindingContext = imagesExerciceViewModel;
        }

        private async void Close_Images(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}