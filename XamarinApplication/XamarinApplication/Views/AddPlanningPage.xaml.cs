﻿using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPlanningPage : PopupPage
    {
        public AddPlanningPage()
        {
            InitializeComponent();
            BindingContext = new AddPlanningViewModel(Navigation);
        }
    }
}