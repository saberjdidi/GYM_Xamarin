using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class CustomerCard
    {
        public long id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public Role role { get; set; }
        public Gim gim { get; set; }
    }
}
