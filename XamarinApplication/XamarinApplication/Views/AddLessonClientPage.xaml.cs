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
    public partial class AddLessonClientPage : PopupPage
    {
        public AddLessonClientPage(User user)
        {
            InitializeComponent();
            var addLessonViewModel = new AddLessonClientViewModel(user);
            addLessonViewModel.User = user;
            BindingContext = addLessonViewModel;
        }
    }
}