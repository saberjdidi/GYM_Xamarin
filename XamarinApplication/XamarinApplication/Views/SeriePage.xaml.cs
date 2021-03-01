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
    public partial class SeriePage : ContentPage
    {
        public SeriePage()
        {
            InitializeComponent();
            BindingContext = new SerieViewModel(Navigation);
        }

        protected override void OnAppearing()
        {
            (this.BindingContext as SerieViewModel).GetSeries();
        }

       /* private async void Serie_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var serie = e.Item as Serie;
            await Navigation.PushAsync(new SerieDetailPage(serie));
        }*/
    }
}