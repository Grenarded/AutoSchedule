//Author: Ben Petlach
//File Name: EventEditForm.cs
//Project Name: AutoSchedule
//Creation Date: June 5, 2023
//Modified Date: June 11, 2023
//Description: A form allowing the user to edit the selected event's information

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
        //Store the original event's information
        private DateTime originalDate;
        private TimeSpan originalTimeStart;
        private TimeSpan originalTimeEnd;
        private string originalEventName;

        //Store a list of events to store under the combo box (if applicable)
        private List<UserControlEvent> comboEvents;

        public EventEditForm() : base()
        {
            InitializeComponent();
        }

        //Edit a displayed event
        public EventEditForm(UserControlEvent selectedEvent) : base()
        {
            InitializeComponent();

            //Set all the form values to that of the selected events
            SetAllValues(selectedEvent);
            
        }

        //Edit a hidden event (under the remaining banner)
        public EventEditForm(List<UserControlEvent> comboEvents)
        {
            InitializeComponent();

            //Store and show combo events
            this.comboEvents = comboEvents;
            gbEventSelection.Visible = true;

            //Add each hidden event name to the combo box)
            foreach (UserControlEvent comboEvent in comboEvents)
            {
                cbMoreEvents.Items.Add(comboEvent.GetEventName());
            }

            //Default combo box selection
            cbMoreEvents.SelectedIndex = 0;

            //Set all the form values to that of the default combo box selection
            SetAllValues(comboEvents[cbMoreEvents.SelectedIndex]);
        }

        //Pre: event
        //Post: None
        //Desc: Update the form to display and store the values of the event passed in
        private void SetAllValues(UserControlEvent selectedEvent)
        {
            //Get all required event values
            date = selectedEvent.GetDate();
            timeStart = selectedEvent.GetTimeStart();
            timeEnd = selectedEvent.GetTimeEnd();
            eventName = selectedEvent.GetEventName();

            //Save original event values
            originalDate = date;
            originalTimeStart = timeStart;
            originalTimeEnd = timeEnd;
            originalEventName = eventName;

            //Update controls to display event values
            datePicker.Value = date;
            timePickerStart.Value = new DateTime(date.Year, date.Month, date.Day, timeStart.Hours, timeStart.Minutes, timeStart.Seconds);
            timePickerEnd.Value = new DateTime(date.Year, date.Month, date.Day, timeEnd.Hours, timeEnd.Minutes, timeEnd.Seconds);
            txtEvent.Text = eventName;
        }

        //Pre: None
        //Post: None
        //Desc: Save the event list to file
        public override void SaveEvent()
        {
            try
            {
                outFile = File.CreateText(Form1.EVENT_FILE);

                //Loop through all the calendar events 
                for (int i = 0; i < Form1.allEvents.Count; i++)
                {
                    //Save just the date portion of the date and not the default time value
                    string dateOnly = Convert.ToString(Form1.allEvents[i].GetDate()).Split(' ')[0];

                    //Write out event along with all its information to file
                    outFile.WriteLine(dateOnly + "," + Form1.allEvents[i].GetTimeStart() + "," + Form1.allEvents[i].GetTimeEnd() + "," + Form1.allEvents[i].GetEventName());
                }
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

        public override void btnSave_Click(object sender, EventArgs e)
        {
            //Check if the end time is valid
            if (IsEndTimeValid())
            {
                //Hide end time invalid label 
                lblEndTimeError.Visible = false;

                //Enable save button
                btnSave.Enabled = true;

                //Save the original event and the edited event
                UserControlEvent originalEvent = new UserControlEvent(originalDate, originalTimeStart, originalTimeEnd, originalEventName);
                UserControlEvent editedEvent = new UserControlEvent(date, timeStart, timeEnd, eventName);

                //Delete the original event
                Form1.DeleteEvent(originalEvent);

                //Add and save the edited evet
                Form1.AddEvent(editedEvent);
                SaveEvent();

                Close();
            }
            else
            {
                //Display error label for invalid end time
                lblEndTimeError.Visible = true;
                btnSave.Enabled = false;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Delete event
            Form1.DeleteEvent(new UserControlEvent(originalDate, originalTimeStart, originalTimeEnd, originalEventName));
            SaveEvent();

            Close();
        }

        private void cbMoreEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Update values to match that of the currently selected combo box item
            SetAllValues(comboEvents[cbMoreEvents.SelectedIndex]);
        }
    }
}
