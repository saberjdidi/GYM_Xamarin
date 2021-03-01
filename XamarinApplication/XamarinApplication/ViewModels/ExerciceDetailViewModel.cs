using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XamarinApplication.Models;

namespace XamarinApplication.ViewModels
{
    public class ExerciceDetailViewModel
    {
        public INavigation Navigation { get; set; }
        public ExerciceDetailViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
        }
        public Exercice Exercice { get; set; }
    }
}
