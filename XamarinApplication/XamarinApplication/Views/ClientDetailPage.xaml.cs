using BottomBar.XamarinForms;
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
    public partial class ClientDetailPage : BottomBarPage
    {
        public ClientDetailPage(User user)
        {
            InitializeComponent();

            var clientViewModel = new ClientDetailViewModel(Navigation);
            clientViewModel.User = user;
            BindingContext = clientViewModel;
        }
    }
}