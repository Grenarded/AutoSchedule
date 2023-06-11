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
            timeStart = RoundTime(timePickerStart);
            timePickerEnd.Value = timePickerStart.Value.AddHours(1);
            timeEnd = RoundTime(timePickerEnd);
        }

        private void EventForm_Load(object sender, EventArgs e)
        {
            datePicker.Value = date;

            if (!IsEndTimeValid())
            {
                lblEndTimeError.Visible = true;
                btnSave.Enabled = false;
            }
        }

        protected void SetDate(DateTime date)
        {
            this.date = date;
        }

        private  void SetTime(TimeSpan time, bool isTimeStart)
        {
            if (IsEndTimeValid())
            {
                lblEndTimeError.Visible = false;

                if (txtEvent.TextLength > 0)
                {
                    btnSave.Enabled = true;
                }
            }
            else
            {
                lblEndTimeError.Visible = true;
                btnSave.Enabled = false;
            }

            //Update start and end times
            if (isTimeStart)
            {
                timeStart = time;
            }
            else
            {
                timeEnd = time;
            }
        }

        protected bool IsEndTimeValid()
        {
            return timePickerEnd.Value > timePickerStart.Value; 
        }

        public virtual void btnSave_Click(object sender, EventArgs e)
        {
            if (IsEndTimeValid())
            {
                SaveEvent();
                Form1.AddEvent(new UserControlEvent(date, timeStart, timeEnd, eventName));
                Close();
            }
        }

        //TODO: method can be called a lot less
        private TimeSpan RoundTime(DateTimePicker timePicker)
        {
            return new TimeSpan(timePicker.Value.TimeOfDay.Hours, timePicker.Value.TimeOfDay.Minutes, 0);
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
            catch(FileNotFoundException fnf)
            {
                MessageBox.Show(fnf.Message, "Event Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "Event Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
