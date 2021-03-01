using Rg.Plugins.Popup.Pages;
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
    public partial class SerieDetailPage : PopupPage
    {
        public SerieDetailPage(Serie serie)
        {
            InitializeComponent();

            var serieViewModel = new SerieDetailViewModel(Navigation);
            serieViewModel.Serie = serie;
            BindingContext = serieViewModel;

           // InformationsTitle.Title = Languages.Informations;
           // ExerciceTitle.Title = Languages.Exercice;
        }
        private async void Close_Serie_Details(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}