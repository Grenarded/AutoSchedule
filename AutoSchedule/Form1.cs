//Author: Ben Petlach
//File Name: Form1.cs
//Project Name: AutoSchedule
//Creation Date: May 15, 2023
//Modified Date: June 12, 2023
//Description: A calendar/scheduling app that displays your events in both a monthly and daily view
//
//COURSE CONTENT APPLICATION//
//2D arrays and lists: use of jagged arrays when constructing daily view, as well as lists to store and manage events
//File IO: read in events from a file and save them to file, with error handling
//OOP: its winforms... In all seriousness: a year is composed of months which are composed days which are composed of events. Parent-child relationship with the event add and edit forms
//Recursion: merge sorting, as well as binary search (for first index, last index, and a specific index)
//Sorting and Searching: Merge sort (used when reading in unsorted file) and insertion sort (when adding to the sorted list of events after loading the form). Searching is done when deleting events 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace AutoSchedule
{
    public partial class Form1 : Form
    {
        //File IO
        public const string EVENT_FILE = "Events.txt";

        private StreamReader inFile;
        private StreamWriter outFile;

        //Store all the events throughout the entire calendar
        public static List<UserControlEvent> allEvents = new List<UserControlEvent>();

        //Maintain the current/selected year and month
        public static int yearNum { get; private set; }
        public static int monthNum { get; private set; }
        
        public static Year year { get; private set; }

        //Store the currently selected day
        public static UserControlDay activeDay;

        //Load schedule view
        private ScheduleForm scheduleForm;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Set up tool tips
            ttAddEvent.SetToolTip(btnAddEvent, "Add Event");
            ttDailyView.SetToolTip(btnDailyView, "Daily View");

            //Assign current month and year ints
            monthNum = DateTime.Now.Month;
            yearNum = DateTime.Now.Year;

            //Clear events if form is being loaded again (from schedule view)
            allEvents.Clear();

            //Read in all the events
            ReadEvents();

            //Instantiate year from current year
            year = new Year(yearNum);

            //Set active day to the current day
            activeDay = year.GetMonth(monthNum).GetDay(DateTime.Now.Day);

            //Display all the days and their info
            DisplayDays();

            //Load the schedule form behind the scenes
            LoadScheduleForm();
        }

        //Pre: None
        //Post: None
        //Desc: Instantiate and open the schedule form for more seamless user access
        private void LoadScheduleForm()
        {
            //Instantiate schedule form
            scheduleForm = new ScheduleForm(activeDay, monthNum, yearNum, this);

            //Store original form size
            Size originalSize = scheduleForm.Size;

            //Decrease size so user doesn't see form
            scheduleForm.Size = new Size(0, 0);

            //Make the form visible and invisible so user doesn't see it
            scheduleForm.Visible = true;
            scheduleForm.Visible = false;

            //Resize form back to original size
            scheduleForm.Size = originalSize;
        }

        //Pre: None
        //Post: None
        //Desc: Read in the events file and store the events
        private void ReadEvents()
        {
            bool removeErrors = false;
            try
            {
                //Store the data that is read in
                string line;
                string[] data;

                inFile = File.OpenText(EVENT_FILE);

                //Keep count of the number of event read errors
                int numErrors = 0;

                //Loop until the end of the file is reached
                while (!inFile.EndOfStream)
                {
                    //Store the line's information
                    line = inFile.ReadLine();
                    data = line.Split(',');

                    try
                    {
                        //Convert and store each part of the line as the appropriate parameter used to add a new event
                        DateTime date = Convert.ToDateTime(data[0]);
                        TimeSpan timeStart = TimeSpan.Parse(data[1]);
                        TimeSpan timeEnd = TimeSpan.Parse(data[2]);
                        string eventName = data[3];

                        //Add the event to the main event list
                        allEvents.Add(new UserControlEvent(date, timeStart, timeEnd, eventName));
                    }
                    catch
                    {
                        //Increase error num if there's an issue reading in an event
                        numErrors++;
                    }
                }
                if (numErrors > 0)
                {
                    if (MessageBox.Show("Unable to load " + numErrors + " event(s) \nWould you like to resolve this issue?", "Failed Event Load", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        if (MessageBox.Show("This will remove the events that couldn't be loaded and their associated data. \nAre you sure?", "Remove Failed Events", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            removeErrors = true;
                            //Set a variable to true
                            //Check for this variable after all events have been read and saved
                            //Rewrite the entire list to file
                        }
                    }
                    else
                    {
                        // user clicked no
                    }
                    //Display message box warning users that some events couldn't be read/loaded
                    //MessageBox.Show("Unable to load " + numErrors + " event(s)", "Failed Event Load", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                }
            }
            catch(FileLoadException fle)
            {
                //Display error message box
                MessageBox.Show(fle.Message, "File Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch(Exception e)
            {
                //Display error message box
                MessageBox.Show(e.Message, "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //Check if file was previously accessed
                if (inFile != null)
                {
                    //Close file
                    inFile.Close();
                }
            }

            //Check if user chose to remove file read errors
            if (removeErrors)
            {
                ExportEvents();
            }

            //Sort all the events
            allEvents = MergeSort(allEvents);
        }

        //Pre: None
        //Post: None
        //Desc: Overrwrite events file with the event list that took in only the acceptable events 
        private void ExportEvents()
        {
            try
            {
                outFile = File.CreateText(EVENT_FILE);

                //Loop through all the events
                for (int i = 0; i < allEvents.Count; i++)
                {
                    //Get only the date portion of the event date (not the associated time)
                    string dateOnly = Convert.ToString(allEvents[i].GetDate()).Split(' ')[0];

                    //Write out event along with all its information to file
                    outFile.WriteLine(dateOnly + "," + allEvents[i].GetTimeStart() + "," + allEvents[i].GetTimeEnd() + "," + allEvents[i].GetEventName());
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
        
        //Pre: List of events
        //Post: sorted list of events
        //Desc: Sort the list of events through left and ride side components
        private List<UserControlEvent> MergeSort(List<UserControlEvent> events)
        {
            //Check if list is already sorted (1 or less events)
            if (events.Count <= 1)
            {
                return events;
            }

            //Store left and right sides of the list
            List<UserControlEvent> left = new List<UserControlEvent>();
            List<UserControlEvent> right = new List<UserControlEvent>();

            //Store the middle index of the list
            int middle = events.Count / 2;

            //Loop through the original list until the middle idnex
            for (int i = 0; i < middle; i++)
            {
                //Add to the left side list
                left.Add(events[i]);
            }

            //Loop through the original list from the middle index to the last index
            for (int i = middle; i < events.Count; i++)
            {
                //Add to the right side list
                right.Add(events[i]);
            }

            //Break up left and right side lists
            left = MergeSort(left);
            right = MergeSort(right);

            //Sort left and right side lists
            return Merge(left, right);
        }

        //Pre: two lists of events (left and right)
        //Post: sorted list of events
        //Desc: Sort list of events by date and start time
        private List<UserControlEvent> Merge(List<UserControlEvent> left, List<UserControlEvent> right)
        {
            //Store sorted list
            List<UserControlEvent> result = new List<UserControlEvent>();

            //Loop while either the left list or right list have more than one object (event)
            while (left.Count > 0 || right.Count > 0)
            {
                //Check if both left and right lists have more than one element
                if (left.Count > 0 && right.Count > 0)
                {
                    //Check if the left list event has a lower value than the right list event (lower value means either lower date, or same date and lower time)
                    if (left[0].GetDate() < right[0].GetDate() || (left[0].GetDate() == right[0].GetDate() && left[0].GetTimeStart() < right[0].GetTimeStart()))
                    {
                        //Add to the sorted list and remove from left list
                        result.Add(left[0]);
                        left.Remove(left[0]);
                    }
                    else
                    {
                        //Add to the sorted list and remove from right list
                        result.Add(right[0]);
                        right.Remove(right[0]);
                    }
                }
                else if (left.Count > 0)
                {
                    //Add to the sorted list and remove from left list
                    result.Add(left[0]);
                    left.Remove(left[0]);
                }
                else if (right.Count > 0)
                {
                    //Add to the sorted list and remove from right list
                    result.Add(right[0]);
                    right.Remove(right[0]);
                }
            }
            return result;
        }

        //Pre: List of events to be sorted, event to be added
        //Post: None
        //Desc: Insert the event into the appropriate spot in a list
        public static void InsertionSort(List<UserControlEvent> eventList, UserControlEvent addedEvent)
        {
            //Add the event to the end of the list
            eventList.Add(addedEvent);

            //Check if the list isn't sorted (greater than one)
            if (eventList.Count > 1)
            {
                //Loop through the list starting from index 1 (index 0 is already sorted)
                for (int i = 1; i < eventList.Count; i++)
                {
                    int j = i;

                    //Loop from the right side of the list to the left and while event isn't sorted 
                    while (j > 0 && eventList[j].GetDateAndTimeStart() < eventList[j - 1].GetDateAndTimeStart())
                    {
                        //Store event as temp
                        UserControlEvent temp = eventList[j];

                        //Swap events
                        eventList[j] = eventList[j - 1];
                        eventList[j - 1] = temp;

                        j--;
                    }
                }
            }
        }

        //Pre: Event to add
        //Post: None
        //Desc: Add an event to the corresponding day
        public static void AddEvent(UserControlEvent newEvent)
        {
            //Get the day to add the event to 
            UserControlDay day = year.GetMonth(newEvent.GetDate().Month).GetDay(newEvent.GetDate().Day);

            //Insert sort the event into the main event list, and add it to the day's list
            InsertionSort(allEvents, newEvent);
            day.AddEvent(newEvent);
        }

        //Pre: Event to delete
        //Post: None
        //Desc: Delete the event from the calendar
        public static void DeleteEvent(UserControlEvent deleteEvent)
        {
            //Get the day to remove the event from
            UserControlDay day = year.GetMonth(deleteEvent.GetDate().Month).GetDay(deleteEvent.GetDate().Day);

            //Delete event from the day
            day.DeleteEvent(deleteEvent);

            //Delete the event from the main list
            allEvents.RemoveAt(day.SearchEvent(allEvents, deleteEvent));
        }

        //Pre: None
        //Post: None
        //Desc: Display all the days and their corresponding info
        private void DisplayDays()
        {
            //Clear the flowlayout panel
            flpDays.Controls.Clear();

            //Add all the days to the flowlayout panel
            flpDays.Controls.AddRange(year.GetMonth(monthNum).GetDayControls()); //TODO: doesn't preload as expected, unless already been loaded before

            //Update label showing the current month and year
            lblMonthYear.Text = year.GetMonth(monthNum).GetMonthName() + " " + yearNum;
        }

        //Pre: None
        //Post: None
        //Desc: Display the event add form
        private void ShowEventForm()
        {
            //Check if there is an active day
            if (activeDay != null)
            {
                //Display event add form as a dialog
                EventForm eventForm = new EventForm(new DateTime(yearNum, monthNum, activeDay.GetDayNum()));
                eventForm.ShowDialog();
            }
        }

        private void btnNextMonth_Click(object sender, EventArgs e)
        {
            //Clear container
            flpDays.Controls.Clear();

            //check if year needs to be incrimented too 
            if (monthNum + 1 > Year.MONTHS_IN_YEAR)
            {
                //Update year and reset month
                yearNum++;
                monthNum = 0;
                year = new Year(yearNum);
            }

            //Increment month
            monthNum++;

            //Refresh and display days
            DisplayDays();
        }

        private void btnPrevMonth_Click(object sender, EventArgs e)
        {
            //Clear container
            flpDays.Controls.Clear();

            //check if year needs to be decreased too 
            if (monthNum - 1 < 1)
            {
                //Update year and reset month
                yearNum--;
                monthNum = Year.MONTHS_IN_YEAR + 1;
                year = new Year(yearNum);
            }

            //Decrement month
            monthNum--;

            //Refresh and display days
            DisplayDays();
        }

        private void btnAddEvent_Click(object sender, EventArgs e)
        {
            //Show the event form
            ShowEventForm();
        }

        private void btnDailyView_Click(object sender, EventArgs e)
        {
            //Update schedule form 
            scheduleForm.SetDay(activeDay);
            scheduleForm.SetMonth(monthNum);
            scheduleForm.SetYear(yearNum);

            scheduleForm.ReloadSchedule();

            //Set the schedule form's position to be the same
            scheduleForm.Location = Location;
            scheduleForm.StartPosition = FormStartPosition.Manual;

            //Make schedule form visible and this one invisible
            scheduleForm.Visible = true;
            Visible = false;
        }
    }
}
