using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace AutoSchedule
{
    public partial class EventForm : Form
    {
        protected DateTime date;
        protected TimeSpan timeStart;
        protected TimeSpan timeEnd;
        protected string eventName;

        //File IO
        protected static StreamWriter outFile;

        public EventForm()
        {
            InitializeComponent();
            date = DateTime.Now;
        }

        public EventForm(DateTime date)
        {
            InitializeComponent();
            this.date = date.Date;
            timeStart = timePickerStart.Value.TimeOfDay;
            timePickerEnd.Value = timePickerStart.Value.AddHours(1);
            timeEnd = timePickerEnd.Value.TimeOfDay;
        }

        //public EventForm(DateTime date, UserControlEvent selectedEvent)
        //{
        //    InitializeComponent();
        //    this.date = date;
        //    timeStart = date.TimeOfDay;
        //    timeEnd = selectedEvent.GetTimeEnd();
        //    eventName = selectedEvent.GetEventName();

        //    //Preset time picker values
        //    timePickerStart.Value = date;
        //    timePickerEnd.Value = new DateTime(date.Year, date.Month, date.Day, timeEnd.Hours, timeEnd.Minutes, timeEnd.Seconds);
        //    txtEvent.Text = eventName;
        //}

        private void EventForm_Load(object sender, EventArgs e)
        {
            datePicker.Value = date;
        }

        protected void SetDate(DateTime date)
        {
            this.date = date;
        }

        protected void SetTime(TimeSpan time, bool isTimeStart)
        {
            if (IsEndTimeValid())
            {
                lblEndTimeError.Visible = false; 
                if (isTimeStart)
                {
                    timeStart = time;
                }
                else
                {
                    timeEnd = time;
                }
            }
            else
            {
                lblEndTimeError.Visible = true;
            }
        }

        protected bool IsEndTimeValid()
        {
            return timePickerEnd.Value > timePickerStart.Value; //TODO: what if the end time is for another day?
        }

        public virtual void btnSave_Click(object sender, EventArgs e)
        {
            if (IsEndTimeValid())
            {
                SaveEvent();
                //TODO: Remove. Insertion sort will eventually be done in driver class
                // Form1.allEvents.Add(new UserControlEvent(date, timeStart, timeEnd, eventName));
                Form1.AddEvent(new UserControlEvent(date, timeStart, timeEnd, eventName));
                //TODO: save event to respective User Control Day
                Close();
            }
            //TODO: if spammed 5 times, error popup window
        }

        public virtual void SaveEvent()
        {
            try
            {
                try
                {
                    //Appends to the existing event file
                    outFile = File.AppendText(Form1.EVENT_FILE);
                }
                catch
                {
                    //Create file (or overwrite if it already exists)
                    outFile = File.CreateText(Form1.EVENT_FILE);
                }

                //Save just the date portion of the date and not the default time value
                string dateOnly = Convert.ToString(date).Split(' ')[0];

                outFile.WriteLine(dateOnly + "," + timeStart + "," + timeEnd + "," + txtEvent.Text);
            }
            catch
            {
                //TODO: window pop up?
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

        private void txtEvent_TextChanged(object sender, EventArgs e)
        {
            if (txtEvent.TextLength > 0)
            {
                btnSave.Enabled = true;
            }
            else
            {
                btnSave.Enabled = false;
            }
            eventName = txtEvent.Text;
        }

        private void datePicker_ValueChanged(object sender, EventArgs e)
        {
            SetDate(datePicker.Value.Date);
        }

        private void timePicker_ValueChanged(object sender, EventArgs e)
        {
            SetTime(timePickerStart.Value.TimeOfDay, true);
        }

        private void timePickerEnd_ValueChanged(object sender, EventArgs e)
        {
            SetTime(timePickerEnd.Value.TimeOfDay, false);
        }
    }
}
