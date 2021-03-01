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
    public partial class ExercicePage : ContentPage
    {
        public ExercicePage()
        {
            InitializeComponent();

            BindingContext = new ExerciceViewModel();
        }
        //load more items
        private void ListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            (BindingContext as ExerciceViewModel).LoadMoreItems(e.Item as Exercice);
        }

        private async void Exercice_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var exercice = e.Item as Exercice;
            await Navigation.PushAsync(new ExerciceDetailPage(exercice));
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
        private async void Update_Exercice(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var exercice = mi.CommandParameter as Exercice;
            await PopupNavigation.Instance.PushAsync(new UpdateExercicePage(exercice));
        }

        private async void Add_Exercice(object sender, EventArgs e)
        {
            // await Navigation.PushAsync(new AddGimPage());
            await PopupNavigation.Instance.PushAsync(new AddExercicePage());
        }
    }
}