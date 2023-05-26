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

        //File IO
        static StreamWriter outFile;

        public EventForm(DateTime date)
        {
            InitializeComponent();
            this.date = date;
        }

        private void EventForm_Load(object sender, EventArgs e)
        {
            datePicker.Value = date;
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

                outFile.WriteLine(date + "," + txtEvent.Text);
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
    }
}
