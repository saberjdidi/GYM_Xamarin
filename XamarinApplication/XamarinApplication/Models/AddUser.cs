using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class AddUser
    {
        public string username { get; set; }
        public string password { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public Role role { get; set; }
        public Gim gim { get; set; }
        public User coach { get; set; }
        public bool enabled { get; set; }
        public string sex { get; set; }
        public string phone { get; set; }
        public DateTime birthdate { get; set; }
        public string height { get; set; }
        public string frequency { get; set; }
        public string bia { get; set; }
        public string note { get; set; }

    }
}
