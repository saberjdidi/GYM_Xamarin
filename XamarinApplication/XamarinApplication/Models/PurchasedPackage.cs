using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class PurchasedPackage
    {
        public long id { get; set; }
        public decimal hour { get; set; }
        public DateTime purchasedDate { get; set; }
        public Machine machine { get; set; }
        public decimal totalConsumed { get; set; }
        public string deleted { get; set; }
    }
}
