using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class AddExercice
    {
        public string level { get; set; }
        public ExerciceTypology exerciseTypologie { get; set; }
        public string name { get; set; }
        public int totalSeries { get; set; }
        public int totalReputation { get; set; }
        public int breakDuration { get; set; }
        public long speed { get; set; }
        public bool concentric { get; set; }
        public int lood { get; set; }
        public int secondLood { get; set; }
        public string typeLoad { get; set; }
        public bool eccentric { get; set; }
        public int loodEccentric { get; set; }
        public int secondLoodEccentric { get; set; }
    }
}
