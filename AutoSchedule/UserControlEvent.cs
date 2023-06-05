using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoSchedule
{
    public partial class UserControlEvent : UserControl
    {
        DateTime dateAndTimeStart;
        TimeSpan timeEnd;
        string eventName;

        public UserControlEvent(DateTime date, TimeSpan timeStart, TimeSpan timeEnd, string eventName)
        {
            dateAndTimeStart = new DateTime(date.Year, date.Month, date.Day, timeStart.Hours, timeStart.Minutes, timeStart.Seconds);
           
            //this.date = date;
            //this.timeStart = timeStart;
            this.timeEnd = timeEnd;
            this.eventName = eventName;

            InitializeComponent();

            DisplayEventName();
        }

        public DateTime GetDateAndTimeStart()
        {
            return dateAndTimeStart;
        }

        public DateTime GetDate()
        {
            return dateAndTimeStart.Date;
        }

        public TimeSpan GetTimeStart()
        {
            return dateAndTimeStart.TimeOfDay;
        }

        public TimeSpan GetTimeEnd()
        {
            return timeEnd;
        }

        public string GetEventName()
        {
            return eventName;
        }

        private void DisplayEventName()
        {
            lblEventName.Text = eventName;
        }
    }
}
