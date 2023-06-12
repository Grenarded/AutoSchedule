//Author: Ben Petlach
//File Name: EventForm.cs
//Project Name: AutoSchedule
//Creation Date: May 16, 2023
//Modified Date: June 10, 2023
//Description: A form allowing the user to add an event to a selected day

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
        //Store the event's information
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

            //Set default event values
            this.date = date.Date;
            timeStart = RoundTime(timePickerStart);
            timePickerEnd.Value = timePickerStart.Value.AddHours(1);
            timeEnd = RoundTime(timePickerEnd);
        }

        protected void SetDate(DateTime date)
        {
            this.date = date;
        }

        //Pre: Time as a timespan, bool indicating whether the time passed in is the start time
        //Post: None
        //Desc: Set the time picker time
        private void SetTime(TimeSpan time, bool isTimeStart)
        {
            //Check if the end time is valid
            if (IsEndTimeValid())
            {
                //Disable warning label
                lblEndTimeError.Visible = false;

                //Check if the event name is more than 0 characters
                if (txtEvent.TextLength > 0)
                {
                    //Enable save button
                    btnSave.Enabled = true;
                }
            }
            else
            {
                //Enable warning label
                lblEndTimeError.Visible = true;

                //Disable save button
                btnSave.Enabled = false;
            }

            //Check if the time passed in is the start or end time, and update it
            if (isTimeStart)
            {
                timeStart = time;
            }
            else
            {
                timeEnd = time;
            }
        }

        //Pre: None
        //Post: None
        //Desc: Save the event list to file
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

                //Write out event along with all its information to file
                outFile.WriteLine(dateOnly + "," + timeStart + "," + timeEnd + "," + txtEvent.Text);
            }
            catch (FileNotFoundException fnf)
            {
                //Display error message box
                MessageBox.Show(fnf.Message, "Event Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception e)
            {
                //Display error message box
                MessageBox.Show(e.Message, "Event Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //Check if file was previously accessed
                if (outFile != null)
                {
                    //Close the file
                    outFile.Close();
                }
            }
        }

        //Pre: None
        //Post: None
        //Desc: Checks if the end time is valid
        protected bool IsEndTimeValid()
        {
            return timePickerEnd.Value > timePickerStart.Value; 
        }

        //Pre: DateTimePicker of timepicker to round the value for
        //Post: a rounded timespan
        //Desc: Rounds the timespan from a given DateTimePicker
        private TimeSpan RoundTime(DateTimePicker timePicker)
        {
            return new TimeSpan(timePicker.Value.TimeOfDay.Hours, timePicker.Value.TimeOfDay.Minutes, 0);
        }

        public virtual void btnSave_Click(object sender, EventArgs e)
        {
            //Check if the end time is valid
            if (IsEndTimeValid())
            {
                //Save the event and add it to the main event list
                SaveEvent();
                Form1.AddEvent(new UserControlEvent(date, timeStart, timeEnd, eventName));

                Close();
            }
        }

        private void EventForm_Load(object sender, EventArgs e)
        {
            //Update date picker value
            datePicker.Value = date;

            //Chek if the end time is valid
            if (!IsEndTimeValid())
            {
                //Display end time warning label 
                lblEndTimeError.Visible = true;

                //Disable save button
                btnSave.Enabled = false;
            }
        }

        private void txtEvent_TextChanged(object sender, EventArgs e)
        {
            if (txtEvent.TextLength > 0 && IsEndTimeValid())
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
            SetTime(RoundTime(timePickerStart), true);
        }

        private void timePickerEnd_ValueChanged(object sender, EventArgs e)
        {
            SetTime(RoundTime(timePickerEnd), false);
        }
    }
}
