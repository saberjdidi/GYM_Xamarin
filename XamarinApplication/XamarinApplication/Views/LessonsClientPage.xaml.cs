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
    public partial class LessonsClientPage : PopupPage
    {
        public LessonsClientPage(User user)
        {
            InitializeComponent();
            var lessonsViewModel = new LessonsClientViewModel();
            lessonsViewModel.User = user;
            BindingContext = lessonsViewModel;
        }
        private async void Close_Lessons(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}