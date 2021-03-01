using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class ProgramDetails
    {
        #region Properties
        public int id { get; set; }
        public string name { get; set; }
        public List<Machine> machines { get; set; }
        public List<Exercice> exercises { get; set; }
        public User coach { get; set; }
        #endregion
    }
}
