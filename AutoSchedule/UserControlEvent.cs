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
        DateTime date;
        TimeSpan timeStart;
        TimeSpan timeEnd;
        string eventName;

        public UserControlEvent(DateTime date, TimeSpan timeStart, TimeSpan timeEnd, string eventName)
        {
            this.date = date;
            this.timeStart = timeStart;
            this.timeEnd = timeEnd;
            this.eventName = eventName;

            InitializeComponent();

            DisplayEventName();
        }

        public DateTime GetDate()
        {
            return date.Date;
        }

        public TimeSpan GetTimeStart()
        {
            return timeStart;
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
