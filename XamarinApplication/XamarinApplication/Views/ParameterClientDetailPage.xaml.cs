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
    public partial class ParameterClientDetailPage : PopupPage
    {
        public ParameterClientDetailPage(ParametreSportif parametreSportif)
        {
            InitializeComponent();
            var parameterViewModel = new ParameterClientDetailViewModel(Navigation);
            parameterViewModel.Parameter = parametreSportif;
            BindingContext = parameterViewModel;
        }
        private async void Close_Parameter_Details(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}