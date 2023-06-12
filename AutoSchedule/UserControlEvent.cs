//Author: Ben Petlach
//File Name: UserControlEvent.cs
//Project Name: AutoSchedule
//Creation Date: May 17, 2023
//Modified Date: June 11, 2023
//Description: Store and manage a single event and its information 

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
        //Store event info
        private DateTime dateAndTimeStart;
        private TimeSpan timeEnd;
        private string eventName;

        public UserControlEvent(DateTime date, TimeSpan timeStart, TimeSpan timeEnd, string eventName)
        {
            //Set the date and time start DateTime
            dateAndTimeStart = new DateTime(date.Year, date.Month, date.Day, timeStart.Hours, timeStart.Minutes, timeStart.Seconds);

            this.timeEnd = timeEnd;
            this.eventName = eventName;

            InitializeComponent();

            //Display the event name
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

        //Pre: None
        //Post: None
        //Desc: Updates the event label with the event's name
        private void DisplayEventName()
        {
            lblEventName.Text = eventName;
        }

        private void lblEventName_Click(object sender, EventArgs e)
        {
            //Show the event edit form
            EventEditForm eventEditForm = new EventEditForm(this);
            eventEditForm.ShowDialog();
        }

        private void lblEventName_MouseHover(object sender, EventArgs e)
        {
            //Change the mouse cursor icon
            Cursor = Cursors.Hand;

            //Create and store DateTimes for the time start and end TimeSpans so AM/PM can be displayed
            DateTime timeStart = DateTime.Today.Add(GetTimeStart());
            DateTime timeEnd = DateTime.Today.Add(GetTimeEnd());

            //Set and display the tooltip with the event's information
            ttEventInfo.SetToolTip(lblEventName, "Start Time: " + timeStart.ToString("hh:mm tt") + "\nEnd Time: " + timeEnd.ToString("hh:mm tt"));
        }
    }
}
