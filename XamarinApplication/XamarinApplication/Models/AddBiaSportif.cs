using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace XamarinApplication.Models
{
    public class AddBiaSportif
    {
        public DateTime date { get; set; }
        public string note { get; set; }
        // public MultipartFormDataContent file { get; set; }
        public byte[] file { get; set; }
        public User sportif { get; set; }
    }
}
