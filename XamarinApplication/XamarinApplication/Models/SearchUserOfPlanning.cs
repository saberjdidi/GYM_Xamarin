using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class SearchUserOfPlanning
    {
        //public long id { get; set; }
        //public Role role { get; set; }
        public User coach { get; set; }
        public List<Role> roles { get; set; }
       // public string firstname { get; set; }
    }
}
