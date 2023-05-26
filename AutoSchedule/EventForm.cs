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
        private TimeSpan time;

        //File IO
        static StreamWriter outFile;

        public EventForm(DateTime date)
        {
            InitializeComponent();
            this.date = date.Date;
            time = timePicker.Value.TimeOfDay;
        }

        private void EventForm_Load(object sender, EventArgs e)
        {
            datePicker.Value = date;
        }

        private void SetDate(DateTime date)
        {
            this.date = date;
        }

        private void SetTime(TimeSpan time)
        {
            this.time = time;
        }

        //TODO: only make save button clickable after event input > 0
        private void btnSave_Click(object sender, EventArgs e)
        {
            //TODO: WRITE TO FILE
            SaveEvent();
            Close();
        }

        private void SaveEvent()
        {
            //TODO: sort events by date in the save file?
            try
            {
                //Create file (or overwrite if it already exists)
                outFile = File.CreateText(Form1.EVENT_FILE);

                //Save just the date portion of the date and not the default time value
                string dateOnly = Convert.ToString(date).Split(' ')[0];

                outFile.WriteLine(dateOnly + "," + time + "," + txtEvent.Text);
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
        }

        private void datePicker_ValueChanged(object sender, EventArgs e)
        {
            SetDate(datePicker.Value.Date);
        }

        private void timePicker_ValueChanged(object sender, EventArgs e)
        {
            SetTime(timePicker.Value.TimeOfDay);
        }
    }
}
