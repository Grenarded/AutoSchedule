using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace AutoSchedule
{
    public partial class EventEditForm : EventForm
    {
        private DateTime originalDate;
        private TimeSpan originalTimeStart;
        private TimeSpan originalTimeEnd;
        private string originalEventName;
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

            originalDate = date;
            originalTimeStart = timeStart;
            originalTimeEnd = timeEnd;
            originalEventName = eventName;

            datePicker.Value = date;
            timePickerStart.Value = new DateTime(date.Year, date.Month, date.Day, timeStart.Hours, timeStart.Minutes, timeStart.Seconds);
            timePickerEnd.Value = new DateTime(date.Year, date.Month, date.Day, timeEnd.Hours, timeEnd.Minutes, timeEnd.Seconds);
            txtEvent.Text = eventName;
        }

        public override void btnSave_Click(object sender, EventArgs e)
        {
            if (IsEndTimeValid())
            {
                lblEndTimeError.Visible = false;
                btnSave.Enabled = true;

                //Edit event
                UserControlEvent originalEvent = new UserControlEvent(originalDate, originalTimeStart, originalTimeEnd, originalEventName);
                UserControlEvent editedEvent = new UserControlEvent(date, timeStart, timeEnd, eventName);

                Form1.DeleteEvent(originalEvent);
                Form1.AddEvent(editedEvent);
                SaveEvent();
                Close();
            }
            else
            {
                lblEndTimeError.Visible = true;
                btnSave.Enabled = false;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Delete event
            Form1.DeleteEvent(new UserControlEvent(date, timeStart, timeEnd, eventName));
            SaveEvent();
            Close();
        }

        public override void SaveEvent()
        {
            try
            {
                outFile = File.CreateText(Form1.EVENT_FILE);

                for (int i = 0; i < Form1.allEvents.Count; i++)
                {
                    //Change somehow
                    string dateOnly = Convert.ToString(Form1.allEvents[i].GetDate()).Split(' ')[0];

                    outFile.WriteLine(dateOnly + "," + Form1.allEvents[i].GetTimeStart() + "," + Form1.allEvents[i].GetTimeEnd() + "," + Form1.allEvents[i].GetEventName());
                }
            }
            catch
            {
                //TODO
            }
            finally
            {
                //Check if file was previously accessed
                if (outFile != null)
                {
                    outFile.Close();
                }
            }
        }
    }
}
