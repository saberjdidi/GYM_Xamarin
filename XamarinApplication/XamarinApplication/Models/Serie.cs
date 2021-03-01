using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using XamarinApplication.Services;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Models
{
    public class Serie
    {
        #region Services
        DialogService dialogService;
        #endregion

        #region Properties
        public int id { get; set; }
        public long serie { get; set; }
        public long initials { get; set; }
        public long effortIncrease { get; set; }
        public long secondsIncreaseEffort { get; set; }
        public long limits { get; set; }
        public long timeStaticity { get; set; }
        public Exercice exercise { get; set; }
        #endregion

        #region Constructors
        public Serie()
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
                "Are you sure to delete this Serie ?");
            if (!response)
            {
                return;
            }

            await SeriesExerciceViewModel.GetInstance().Delete(this);
        }
        #endregion
    }
}
