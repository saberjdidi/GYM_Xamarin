using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class AddMachine
    {
        public string code { get; set; }
        public string serialNumber { get; set; }
        public string ip { get; set; }
        public Gim gim { get; set; }
        public string description { get; set; }
        public string note { get; set; }
        public DateTime deliveryDate { get; set; }
        public DateTime activationDate { get; set; }
        public string status { get; set; }
        //public decimal totalPurchased { get; set; }
        // public decimal totalConsumed { get; set; }
        //public DateTime? lastDatePurchased { get; set; }
    }
}
