using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class AddPlanning
    {
        #region Declaration
        private DateTime m_date;
        #endregion

        #region Property
       // public DateTime date { get; set; }
        public DateTime date 
        {
            get { return m_date; }
            set { this.m_date = value; }
        }
        public object programs { get; set; }
        public User sportif { get; set; }
        #endregion

        #region Constructor
        public AddPlanning()
        {
            m_date = System.DateTime.Today;
        }
        #endregion
    }
}
