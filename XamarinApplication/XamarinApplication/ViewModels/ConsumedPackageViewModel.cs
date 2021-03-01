using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
    public class ConsumedPackageViewModel : BaseViewModel
    {
        #region Services
        private RestService restService;
        #endregion

        #region Attributes
        private ObservableCollection<ConsumedPackage> consumedPackages;
        private bool isRefreshing;
        private string filter;
        private List<ConsumedPackage> consumedPackagesList;
        #endregion

        #region Properties
        public ObservableCollection<ConsumedPackage> ConsumedPackages
        {
            get { return consumedPackages; }
            set
            {
                consumedPackages = value;
                OnPropertyChanged();
            }
        }

        public bool IsRefreshing
        {
            get
            {
                return isRefreshing;
            }
            set
            {
                isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public string Filter
        {
            get { return filter; }
            set
            {
                filter = value;
                OnPropertyChanged();
                Search();
            }
        }
        #endregion

        #region Constructors
        public ConsumedPackageViewModel()
        {
            restService = new RestService();
            LoadConsumedPackage();
        }
        #endregion

        #region Methods
        public async void LoadConsumedPackage()
        {
            IsRefreshing = true;
            var connection = await restService.CheckConnection();
            var accesstoken = Settings.AccessToken;

            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Ok");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }

            var response = await restService.GetList<ConsumedPackage>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/consumedPackage",
                 accesstoken);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            consumedPackagesList = (List<ConsumedPackage>)response.Result;
            ConsumedPackages = new ObservableCollection<ConsumedPackage>(consumedPackagesList);
            IsRefreshing = false;
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadConsumedPackage);
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(Search);
            }
        }

        private void Search()
        {
            if (string.IsNullOrEmpty(Filter))
            {
                ConsumedPackages = new ObservableCollection<ConsumedPackage>(consumedPackagesList);
            }
            else
            {
                ConsumedPackages = new ObservableCollection<ConsumedPackage>(
                    consumedPackagesList.Where(
                        l => l.consumedHour.Equals(Filter)));
            }
        }
        #endregion
    }
}
