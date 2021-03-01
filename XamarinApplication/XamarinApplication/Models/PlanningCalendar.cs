using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class PlanningCalendar
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        public List<Program> programs { get; set; }
        public User sportif { get; set; }
    }
}
