using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class SearchRequestByCoach
    {
        public long id { get; set; }
        public User coach { get; set; }
        public List<Role> roles { get; set; }
    }
}
