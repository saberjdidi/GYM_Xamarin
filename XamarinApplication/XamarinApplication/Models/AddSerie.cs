using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class AddSerie
    {
        public long serie { get; set; }
        public long initials { get; set; }
        public long effortIncrease { get; set; }
        public long secondsIncreaseEffort { get; set; }
        public long limits { get; set; }
        public long timeStaticity { get; set; }
        public Exercice exercise { get; set; }
    }
}
