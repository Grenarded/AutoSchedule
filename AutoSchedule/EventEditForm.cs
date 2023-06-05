using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AutoSchedule
{
    public partial class EventEditForm : EventForm
    {
        public EventEditForm() : base()
        {
            InitializeComponent();
        }

        public EventEditForm(UserControlEvent selectedEvent) : base()
        {
            InitializeComponent();
            date = selectedEvent.GetDate();
            timeStart = selectedEvent.GetTimeStart();
            timeEnd = selectedEvent.GetTimeEnd();
            eventName = selectedEvent.GetEventName();

            datePicker.Value = date;
            timePickerStart.Value = new DateTime(date.Year, date.Month, date.Day, timeStart.Hours, timeStart.Minutes, timeStart.Seconds);
            timePickerEnd.Value = new DateTime(date.Year, date.Month, date.Day, timeEnd.Hours, timeEnd.Minutes, timeEnd.Seconds);
            txtEvent.Text = eventName;
        }

        public override void btnSave_Click(object sender, EventArgs e)
        {
            //Edit event
        }
    }
}
