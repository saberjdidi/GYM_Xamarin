using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XamarinApplication.Models;

namespace XamarinApplication.ViewModels
{
    public class PlanningProgramsDetailViewModel
    {
        #region Properties
        public Program Program
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public INavigation Navigation { get; set; }
        public PlanningProgramsDetailViewModel(INavigation _navigation)
        {
            Navigation = _navigation;

        }
        #endregion
    }
}
