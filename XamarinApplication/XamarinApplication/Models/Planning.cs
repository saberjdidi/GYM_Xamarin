using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using XamarinApplication.Services;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Models
{
    public class Planning
    {
        #region Services
        DialogService dialogService;
        #endregion

        #region Properties
        public int id { get; set; }
        public DateTime date { get; set; }
        //public List<Program> programs { get; set; }
        public object programs { get; set; }
        public User sportif { get; set; }
        #endregion

        #region Constructors
        public Planning()
        {
            dialogService = new DialogService();
            programs = new List<Program>();
        }
        #endregion

        #region Commands
        public ICommand DeleteCommand
        {
            get
            {
                return new RelayCommand(Delete);
            }
        }

        async void Delete()
        {
            var response = await dialogService.ShowConfirm(
                "Confirm",
                "Are you sure to delete this Planning ?");
            if (!response)
            {
                return;
            }

            await PlanningViewModel.GetInstance().Delete(this);
        }
        #endregion
    }
}
