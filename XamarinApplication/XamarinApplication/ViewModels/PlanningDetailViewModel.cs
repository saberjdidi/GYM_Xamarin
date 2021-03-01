using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XamarinApplication.Models;
using System.Linq;

namespace XamarinApplication.ViewModels
{
    public class PlanningDetailViewModel : BaseViewModel
    {
        public INavigation Navigation { get; set; }
        bool _isVisibleStatus;
        public bool IsVisibleStatus
        {
            get { return _isVisibleStatus; }
            set
            {
                _isVisibleStatus = value;
                OnPropertyChanged();
            }
        }
        public PlanningDetailViewModel(INavigation _navigation)
        {
            Navigation = _navigation;

            //GetData();
        }
        public Planning Planning { get; set; }
        /*private void GetData()
        {
            if (Planning.programs.Count() == 0)
            {
                IsVisibleStatus = true;
            }
            else
            {
                IsVisibleStatus = false;
            }
        }*/
    }
}
