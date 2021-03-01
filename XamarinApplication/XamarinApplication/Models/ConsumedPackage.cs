using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class ConsumedPackage
    {
        public long id { get; set; }
        public decimal consumedHour { get; set; }
        public DateTime consumedDate { get; set; }
        public PurchasedPackage purchasedPackage { get; set; }
        public User deletedUser { get; set; }
        public DateTime deletedDate { get; set; }
        public bool deleted { get; set; }
    }
}
