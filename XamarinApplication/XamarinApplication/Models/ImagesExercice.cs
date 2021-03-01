using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using XamarinApplication.Services;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Models
{
    public class ImagesExercice
    {
        #region Services
        DialogService dialogService;
        #endregion

        #region Declaration
        private string m_type;
        #endregion

        #region Property
        public int id { get; set; }
        public string name { get; set; }
        public string type
        {
            get { return m_type; }
            set { this.m_type = value; }
        }
        //public string picByte { get; set; }
        public byte[] picByte { get; set; }
        public Exercice exercise { get; set; }
        #endregion

        #region CONSTRUCTOR
        public ImagesExercice()
        {
            //  m_type = "image/png";
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
                "Are you sure to delete this image ?");
            if (!response)
            {
                return;
            }

            await ImagesExerciceViewModel.GetInstance().Delete(this);
        }
        #endregion
    }
}
