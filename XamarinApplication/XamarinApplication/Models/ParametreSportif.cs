using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using XamarinApplication.Services;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Models
{
    public class ParametreSportif
    {
        #region Services
        DialogService dialogService;
        #endregion

        #region Properties
        public int id { get; set; }
        public DateTime date { get; set; }
        public int chest { get; set; }
        public int rightArm { get; set; }
        public int leftArm { get; set; }
        public int life { get; set; }
        public int sides { get; set; }
        public int thighRight { get; set; }
        public int thighLeft { get; set; }
        public string weight { get; set; }
        public User sportif { get; set; }
        #endregion

        #region Constructors
        public ParametreSportif()
        {
            dialogService = new DialogService();
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
                "Are you sure to delete this Parameter ?");
            if (!response)
            {
                return;
            }

           await ParametresClientViewModel.GetInstance().Delete(this);
        }
        #endregion
    }
}
