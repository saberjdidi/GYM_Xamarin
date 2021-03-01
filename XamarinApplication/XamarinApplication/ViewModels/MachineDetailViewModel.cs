using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XamarinApplication.Models;

namespace XamarinApplication.ViewModels
{
    public class MachineDetailViewModel
    {
        public INavigation Navigation { get; set; }
        public MachineDetailViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
        }
        public Machine Machine { get; set; }
    }
}
