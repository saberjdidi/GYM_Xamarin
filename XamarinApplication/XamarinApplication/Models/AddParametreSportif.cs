using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class AddParametreSportif
    {
        public DateTime date { get; set; }
        public int chest { get; set; }
        public int rightArm { get; set; }
        public int leftArm { get; set; }
        public int life { get; set; }
        public int sides { get; set; }
        public int thighRight { get; set; }
        public int thighLeft { get; set; }
        public int weight { get; set; }
        public User sportif { get; set; }
    }
}
