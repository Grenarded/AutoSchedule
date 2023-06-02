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
        private DateTime date;
        private TimeSpan timeStart;
        private TimeSpan timeEnd;
        private string eventName;

        //File IO
        static StreamWriter outFile;

        public EventForm(DateTime date)
        {
            InitializeComponent();
            this.date = date.Date;
            timeStart = timePickerStart.Value.TimeOfDay;
            timePickerEnd.Value = timePickerStart.Value.AddHours(1);
            timeEnd = timePickerEnd.Value.TimeOfDay;
        }

        private void EventForm_Load(object sender, EventArgs e)
        {
            datePicker.Value = date;
        }

        private void SetDate(DateTime date)
        {
            this.date = date;
        }

        private void SetTime(TimeSpan time, bool isTimeStart)
        {
            if (IsEndTimeValid())
            {
                lblEndTimeError.Visible = false; //TODO: doesn't work
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

        private bool IsEndTimeValid()
        {
            return timePickerEnd.Value > timePickerStart.Value; //TODO: what if the end time is for another day?
        }

        //TODO: only make save button clickable after event input > 0
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsEndTimeValid())
            {
                SaveEvent();
                //TODO: insertion sort into list
                Form1.allEvents.Add(new UserControlEvent(date, timeStart, timeEnd, eventName));
                
                //Form1.
                //TODO: save event to respective User Control Day
                Close();
            }
            //TODO: if spammed 5 times, error popup window
        }

        private void SaveEvent()
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
