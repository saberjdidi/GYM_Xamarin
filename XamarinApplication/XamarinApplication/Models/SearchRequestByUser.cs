using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class SearchRequestByUser
    {
        public long id { get; set; }
        public Role role { get; set; }
        public Gim gim { get; set; }
        public List<Role> roles { get; set; }
    }
}
