using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApplication.Tooltip;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddGimPage : PopupPage
    {
        public AddGimPage()
        {
            InitializeComponent();
            BindingContext = new AddGYMViewModel(Navigation);
        }

        void Tooltip_Tapped(object sender, System.EventArgs e)
        {

            foreach (var c in mainLayout.Children)
            {
                if (TooltipEffect.GetHasTooltip(c))
                {
                    TooltipEffect.SetHasTooltip(c, false);
                    TooltipEffect.SetHasTooltip(c, true);
                }
            }
        }
    }
}