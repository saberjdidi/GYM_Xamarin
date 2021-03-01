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
    public class PurchasedPackageViewModel : BaseViewModel
    {
        #region Services
        private RestService restService;
        #endregion

        #region Attributes
        private ObservableCollection<PurchasedPackage> purchased;
        private bool isRefreshing;
        private string filter;
        private List<PurchasedPackage> purchasedsList;
        #endregion

        #region Properties
        public ObservableCollection<PurchasedPackage> Purchaseds
        {
            get { return purchased; }
            set
            {
                purchased = value;
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
        public PurchasedPackageViewModel()
        {
            restService = new RestService();
            LoadPurchased();
        }
        #endregion

        #region Methods
        public async void LoadPurchased()
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

            var response = await restService.GetList<PurchasedPackage>(
                 "http://phoneofficine.it",
                 "/niini-gim",
                 "/purchasedPackage",
                 accesstoken);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            purchasedsList = (List<PurchasedPackage>)response.Result;
            Purchaseds = new ObservableCollection<PurchasedPackage>(purchasedsList);
            IsRefreshing = false;
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadPurchased);
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
                Purchaseds = new ObservableCollection<PurchasedPackage>(purchasedsList);
            }
            else
            {
                Purchaseds = new ObservableCollection<PurchasedPackage>(
                    purchasedsList.Where(
                        l => l.machine.code.ToLower().Contains(Filter.ToLower())));
            }
        }
        #endregion
    }
}
