using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.ViewModels
{
    using Models;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using Xamarin.Forms;

    public class ProgramDetailViewModel : BaseViewModel
    {

        #region Properties
        public Program Program {
            get;
            set;
        }

        #endregion

        #region Constructors
        /* first method
        public ProgramDetailViewModel(Program program)
        {
            this.Program = program;
        }*/
        public INavigation Navigation { get; set; }
        public ProgramDetailViewModel(INavigation _navigation)
        {
            Navigation = _navigation;

        }
        #endregion
        }
}
