using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using XamarinApplication.Services;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Models
{
    public class User
    {
        #region Services
        DialogService dialogService;
        #endregion

        #region Properties
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public Role role { get; set; }
        public Gim gim { get; set; }
        public User coach { get; set; }
        public bool enabled { get; set; }
        public string sex { get; set; }
        public string phone { get; set; }
        public DateTime? birthdate { get; set; }
        public string height { get; set; }
        public string frequency { get; set; }
        public string bia { get; set; }
        public string note { get; set; }
        #endregion

        #region Constructors
        public User()
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
                "Are you sure to delete this Athletic ?");
            if (!response)
            {
                return;
            }

            await ClientViewModel.GetInstance().Delete(this);
        }
        #endregion
    }
}
