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
    public partial class SeriesExercicePage : PopupPage
    {
        public SeriesExercicePage(Exercice exercice)
        {
            InitializeComponent();

            var seriesExerciceViewModel = new SeriesExerciceViewModel();
            seriesExerciceViewModel.Exercice = exercice;
            BindingContext = seriesExerciceViewModel;
        }

        private async void Close_Series(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }

        private async void Update_Serie(object sender, EventArgs e)
        {
            TappedEventArgs tappedEventArgs = (TappedEventArgs)e;
            Serie serie = ((SeriesExerciceViewModel)BindingContext).Series.Where(ser => ser.id == (int)tappedEventArgs.Parameter).FirstOrDefault();
            await PopupNavigation.Instance.PushAsync(new UpdateSeriePage(serie));
        }

        private async void Serie_Details(object sender, ItemTappedEventArgs e)
        {
            var serie = e.Item as Serie;
            await PopupNavigation.Instance.PushAsync(new SerieDetailPage(serie));
        }
    }
}