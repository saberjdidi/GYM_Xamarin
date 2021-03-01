using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using XamarinApplication.Services;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Models
{
    public class Machine
    {
        #region Services
        DialogService dialogService;
        #endregion

        #region Properties
        public int id { get; set; }
        public string code { get; set; }
        public string serialNumber { get; set; }
        public string ip { get; set; }
        public Gim gim { get; set; }
        public string description { get; set; }
        public string note { get; set; }
        public DateTime activationDate { get; set; }
        public DateTime deliveryDate { get; set; }
        public string status { get; set; }
        //public decimal totalPurchased { get; set; }
        //public decimal totalConsumed { get; set; }
        //public DateTime lastDatePurchased { get; set; }
        #endregion

        #region Constructors
        public Machine()
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
                "Are you sure to delete this Machine ?");
            if (!response)
            {
                return;
            }

            await MachineViewModel.GetInstance().Delete(this);
        }
        #endregion

        /*  public override string ToString()
          {
              return $"{totalPurchased:C2}";
          }*/
    }
}
