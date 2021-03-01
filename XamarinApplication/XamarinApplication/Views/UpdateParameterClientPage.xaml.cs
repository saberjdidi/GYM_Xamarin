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
    public partial class UpdateParameterClientPage : PopupPage
    {
        public UpdateParameterClientPage(ParametreSportif parametreSportif)
        {
            InitializeComponent();
            var updateParametreViewModel = new UpdateParameterClientViewModel(Navigation);
            updateParametreViewModel.Parameters = parametreSportif;
            BindingContext = updateParametreViewModel;
        }
    }
}