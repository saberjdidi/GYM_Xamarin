using Rg.Plugins.Popup.Services;
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
    public partial class ExerciceEntreneurPage : ContentPage
    {
        public ExerciceEntreneurPage()
        {
            InitializeComponent();
            BindingContext = new ExerciceEntreneurViewModel();
        }
        private void ListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            (BindingContext as ExerciceEntreneurViewModel).LoadMoreItems(e.Item as Exercice);
        }
        private async void Exercice_Detail(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var exercice = mi.CommandParameter as Exercice;
            await Navigation.PushAsync(new ExerciceDetailPage(exercice));
        }
        private async void Series_Exercice(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var exercice = mi.CommandParameter as Exercice;
            //await Navigation.PushAsync(new SeriesExercicePage(exercice));
            await PopupNavigation.Instance.PushAsync(new SeriesExercicePage(exercice));
        }
        private async void Images_Exercice(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var exercice = mi.CommandParameter as Exercice;
            await PopupNavigation.Instance.PushAsync(new ImagesExercicePage(exercice));
        }
        private async void Add_Exercice(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new AddExercicePage());
        }
    }
}