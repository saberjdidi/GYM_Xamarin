﻿using Rg.Plugins.Popup.Pages;
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
    public partial class UpdateBiaClientPage : PopupPage
    {
        public UpdateBiaClientPage(BiaSportif biaSportif)
        {
            InitializeComponent();
            var updateBiaViewModel = new UpdateBiaClientViewModel(Navigation);
            updateBiaViewModel.Bia = biaSportif;
            BindingContext = updateBiaViewModel;
        }
    }
}