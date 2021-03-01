using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using XamarinApplication.Services;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Models
{
    public class Program
    {
        #region Services
        DialogService dialogService;
        #endregion

        #region Properties
        public int id { get; set; }
        public string name { get; set; }
        public object machines { get; set; }
        public object exercises { get; set; }
        //public List<Machine> machines { get; set; }
        //public List<Exercice> exercises { get; set; }
        public User coach { get; set; }
        #endregion

        #region Constructors
        public Program()
        {
            dialogService = new DialogService();
            machines = new List<Machine>();
            exercises = new List<Exercice>();
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
                "Are you sure to delete this Program ?");
            if (!response)
            {
                return;
            }

            await ProgramViewModel.GetInstance().Delete(this);
        }
        #endregion
    }
}
