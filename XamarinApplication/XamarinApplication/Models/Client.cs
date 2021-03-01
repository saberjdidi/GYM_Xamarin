using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using XamarinApplication.Services;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Models
{
    public class Client
    {
        #region Services
        DialogService dialogService;
        #endregion

        #region Properties
        public int id { get; set; }
        public int number { get; set; }
        public DateTime startDate { get; set; }
        public User customerCard { get; set; }
        #endregion

        #region Constructors
        public Client()
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
                "Are you sure to delete this Lesson ?");
            if (!response)
            {
                return;
            }

            await LessonsClientViewModel.GetInstance().Delete(this);
        }
        #endregion
    }
}
