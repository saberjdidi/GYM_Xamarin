using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using XamarinApplication.Services;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Models
{
    public class BiaSportif
    {
        #region Services
        DialogService dialogService;
        #endregion

        #region Properties
        public int id { get; set; }
        public DateTime date { get; set; }
        public string note { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public byte[] picByte { get; set; }
        public User sportif { get; set; }
        #endregion

        #region Constructors
        public BiaSportif()
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
                "Are you sure to delete this Bia ?");
            if (!response)
            {
                return;
            }

            await BiaClientViewModel.GetInstance().Delete(this);
        }
        #endregion
    }
}
