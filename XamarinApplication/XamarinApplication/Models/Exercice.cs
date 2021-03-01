using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using XamarinApplication.Services;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Models
{
    public class Exercice
    {
        #region Services
        DialogService dialogService;
        #endregion

        #region Properties
        public int id { get; set; }
        public string name { get; set; }
        public int totalSeries { get; set; }
        public int totalReputation { get; set; }
        public int lood { get; set; }
        public int secondLood { get; set; }
        public string typeLoad { get; set; }
        public int breakDuration { get; set; }
        // public decimal speed { get; set; }
        public string speed { get; set; }
        public string statique { get; set; }
        public string level { get; set; }
        public bool concentric { get; set; }
        public bool eccentric { get; set; }
        public int loodEccentric { get; set; }
        public int secondLoodEccentric { get; set; }
        public User coach { get; set; }
        public ExerciceTypology exerciseTypologie { get; set; }
        #endregion

        #region Constructors
        public Exercice()
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
                "Are you sure to delete this Exercice ?");
            if (!response)
            {
                return;
            }

             await ExerciceViewModel.GetInstance().Delete(this);
        }
        #endregion

    }
}
