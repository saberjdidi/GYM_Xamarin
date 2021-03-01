using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class AddGym
    {
        public string code { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string address { get; set; }
        public string tel { get; set; }
        public string email { get; set; }
        public string webSite { get; set; }
        public string referencePerson { get; set; }
        public LicenceGYM licence { get; set; }
    }
}
