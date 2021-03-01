using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using XamarinApplication.Services;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Models
{
    public class Gim
    {
        #region Services
        DialogService dialogService;
        #endregion

        #region Properties
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }
        [JsonProperty(PropertyName = "tel")]
        public string Tel { get; set; }
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "webSite")]
        public string WebSite { get; set; }
        [JsonProperty(PropertyName = "referencePerson")]
        public string ReferencePerson { get; set; }
        [JsonProperty(PropertyName = "licence")]
        public LicenceGYM licence { get; set; }
        
        [JsonProperty(PropertyName = "deliveryDate")]
        public DateTime? DeliveryDate { get; set; }
        [JsonProperty(PropertyName = "activationDate")]
        public DateTime? ActivationDate { get; set; }
        #endregion

        #region Constructors
        public Gim()
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
                "Are you sure to delete this GYM ?");
            if (!response)
            {
                return;
            }

            await GimViewModel.GetInstance().Delete(this);
        }
        #endregion
    }
}
