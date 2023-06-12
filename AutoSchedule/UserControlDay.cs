//Author: Ben Petlach
//File Name: UserControlDay.cs
//Project Name: AutoSchedule
//Creation Date: May 15, 2023
//Modified Date: June 10, 2023
//Description: Manage and display the data within each day (day number and events)

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoSchedule
{
    public partial class UserControlDay : UserControl
    {
        //Max number of events that can be displayed at once on a day
        private const int MAX_EVENTS_DISPLAYED = 4;

        //The day of the month (1-31, depending on month)
        private int dayNum;

        //List of all the events associated with the day
        private List<UserControlEvent> events = new List<UserControlEvent>();

        //Track the remaining events that aren't being displayed when the event max is reached
        int remainingEvents;

        //Check if the day is a blank day or not, where blank day is used as a filler for the flowlayoutpanel before the actual first day of the month starts
        private bool isBlankDay = false;

        //Day with events
        public UserControlDay(int dayNum, List<UserControlEvent> events)
        {
            this.dayNum = dayNum;
            this.events = events;

            InitializeComponent();

            //Add all the events associated with the day
            AddEvents();
        }

        //Day with no events
        public UserControlDay(int dayNum)
        {
            this.dayNum = dayNum;

            InitializeComponent();
        }
        
        //Blank day
        public UserControlDay()
        {
            InitializeComponent();

            //Color and label so that day isn't noticeable
            isBlankDay = true;
            BackColor = DefaultBackColor;
            lblDayNum.Text = "";
        }

        public List<UserControlEvent> GetEvents()
        {
            return events;
        }

        public int GetDayNum()
        {
            return dayNum;
        }

        //Pre: None
        //Post: None
        //Desc: Display the date number (an int from 1-31)
        public void DisplayDate()
        {
            lblDayNum.Text = Convert.ToString(dayNum);
        }

        //Pre: None
        //Post: None
        //Desc: Add the events to this day object
        private void AddEvents()
        {
            //Check if the number of events added is greater than the number of events allowed to be displayed
            if (events.Count > MAX_EVENTS_DISPLAYED)
            {
                //Loop through all the events that can be displayed
                for (int i = 0; i < MAX_EVENTS_DISPLAYED - 1; i++)
                {
                    //Create the event and display it
                    UserControlEvent ucEvent = new UserControlEvent(events[i].GetDate(), events[i].GetTimeStart(), events[i].GetTimeEnd(), events[i].GetEventName());
                    flpEvents.Controls.Add(ucEvent);
                }

                //Display banner indicating how many more events there are 
                remainingEvents = events.Count - MAX_EVENTS_DISPLAYED + 1; //Since the banner takes up an event space, there is actually one more event hidden than the max allowed to be displayed (3 displayed rather than 4)
                lblMaxEvents.Text = "+ " + remainingEvents + " More Events";
                lblMaxEvents.Visible = true;
            }
            else
            {
                //Hide the display banner for more events
                lblMaxEvents.Visible = false;

                //Loop through all the events
                for (int i = 0; i < events.Count; i++)
                {
                    //Create events and display them
                    UserControlEvent ucEvent = new UserControlEvent(events[i].GetDate(), events[i].GetTimeStart(), events[i].GetTimeEnd(), events[i].GetEventName());
                    flpEvents.Controls.Add(ucEvent);
                }
            }
        }
        //Pre: Event to add
        //Post: None
        //Desc: Add a single event
        public void AddEvent(UserControlEvent newEvent)
        {
            //Add and insert the new event into the appropriate sorted position in the main events list
            Form1.InsertionSort(events, newEvent);

            //Clear out the event controls and re-add all events so that they are displayed in a sorted order
            flpEvents.Controls.Clear();
            AddEvents();
        }

        //Pre: Event to delete
        //Post: None
        //Desc: Delete a single event
        public void DeleteEvent(UserControlEvent deleteEvent)
        {
            //Remove event from main list
            events.RemoveAt(SearchEvent(events, deleteEvent));

            //Clear out the event controls and re-add all events so that they are displayed in a sorted order
            flpEvents.Controls.Clear();
            AddEvents();
        }

        //Pre: list of events to search through, the event to search for
        //Post: the index of the searched event in the events list provided
        //Desc: Get the index of a desired event within a list
        public int SearchEvent(List<UserControlEvent> eventsList, UserControlEvent searchEvent)
        {
            //Get the index for an event within the day
            int eventIndex = BinarySearchSpecific(eventsList, searchEvent.GetDateAndTimeStart(), 0, eventsList.Count);

            //Verify if the index is actually the event desired, and if not get and return the actual index
            return VerifySearchEvent(eventsList, searchEvent, eventsList[eventIndex], eventIndex);
        }

        //Pre: list of events of which to get the event index for, the event that was searched, the event that is desired, the index provided by the search as an int
        //Post: an updated index as an int
        //Desc: Verify if the event found by the binary search matches the one that was searched for, and if not get the index for the event that matches
        private int VerifySearchEvent(List<UserControlEvent> eventsList, UserControlEvent searchEvent, UserControlEvent foundEvent, int listIndex)
        {
            //Check if the found event's end time or name match
            if (searchEvent.GetTimeEnd() != foundEvent.GetTimeEnd() || searchEvent.GetEventName() != foundEvent.GetEventName())
            {
                int index = listIndex;

                //Loop from the event after the found event until the end of the event list is reached (work forwards from the found event)
                while (index + 1 < eventsList.Count)
                {
                    index++;

                    //Check if the new event's date and start time matches the searched for event's date and start time
                    if (eventsList[index].GetDateAndTimeStart() == searchEvent.GetDateAndTimeStart())
                    {
                        //Check if the new event's end time and name matches the searched for event's end time and name
                        if (searchEvent.GetTimeEnd() == eventsList[index].GetTimeEnd() && searchEvent.GetEventName() == eventsList[index].GetEventName())
                        {
                            return index;
                        }
                    }
                }

                //Reset the index
                index = listIndex;

                //Loop from the previous event until the beginning of the list is reached (work backwards from the found event)
                while (index-1 >= 0)
                {
                    index--;

                    //Check if the new event's date and start time matches the searched for event's date and start time
                    if (eventsList[index].GetDateAndTimeStart() == searchEvent.GetDateAndTimeStart())
                    {
                        //Check if the new event's end time and name matches the searched for event's end time and name
                        if (searchEvent.GetTimeEnd() == eventsList[index].GetTimeEnd() && searchEvent.GetEventName() == eventsList[index].GetEventName())
                        {
                            return index;
                        }
                    }
                }
            }

            return listIndex;
        }

        //Pre: list of events, the DateTime of the desired event, low and high list indices as ints
        //Post: Index as an int
        //Desc: Recursively search for an event in the list with the date AND time provided
        private int BinarySearchSpecific(List<UserControlEvent> events, DateTime dateTimeStart, int low, int high)
        {
            //Check if the lower bound is greater than the upper bound, the search has failed
            if (low > high)
            {
                return -1;
            }

            //Calculate the middle index of the search range
            int mid = (low + high) / 2;

            //Check if the start time at the middle index matches the date and start time being searched for 
            if (dateTimeStart == events[mid].GetDateAndTimeStart())
            {
                //Check if the middle index is 0 or the start time at the previous index does not match the date and start time being searched fo
                if (mid == 0 || events[mid - 1].GetDateAndTimeStart() != dateTimeStart)
                {
                    return mid;
                }
                else
                {
                    //Continue searching in the lower half of the search range.
                    return BinarySearchSpecific(events, dateTimeStart, low, mid - 1);
                }
            }
            else if (dateTimeStart < events[mid].GetDateAndTimeStart())
            {
                //Continue searching in the lower half of the search range.
                return BinarySearchSpecific(events, dateTimeStart, low, mid - 1);
            }
            else
            {
                //Continue searching in the upper half of the search range.
                return BinarySearchSpecific(events, dateTimeStart, mid + 1, high);
            }
        }

        //Pre: list of events, DateTime date of the desired event, low and high indices as ints
        //Post: index as an int
        //Desc: Find the first occuring event on the specified date
        public int BinarySearchFirstIndex(List<UserControlEvent> events, DateTime date, int low, int high)
        {
            //Check if the lower bound is greater than the upper bound, the search has failed
            if (low > high)
            {
                return -1;
            }

            //Calculate the middle index of the search range
            int mid = (low + high) / 2;

            //Check if the date at the middle index matches the date being searched for
            if (date == events[mid].GetDate())
            {
                //CHeck if the middle index is 0 or the date at the previous index does not match the date being searched for
                if (mid == 0 || events[mid - 1].GetDate() != date)
                {
                    return mid;
                }
                else
                {
                    //Continue searching in the lower half of the search range.
                    return BinarySearchFirstIndex(events, date, low, mid - 1);
                }
            }
            else if (date < events[mid].GetDate())
            {
                //Continue searching in the lower half of the search range.
                return BinarySearchFirstIndex(events, date, low, mid - 1);
            }
            else
            {
                //Continue searching in the upper half of the search range.
                return BinarySearchFirstIndex(events, date, mid + 1, high);
            }
        }

        //Pre: list of events, DateTime date of the desired event, low and high indices as ints
        //Post: index as an int
        //Desc: Find the last occuring event on the specified date
        public int BinarySearchLastIndex(List<UserControlEvent> events, DateTime date, int low, int high)
        {
            //Check if the lower bound is greater than the upper bound, the search has failed
            if (low > high)
            {
                return -1;
            }

            //Calculate the middle index of the search range
            int mid = (low + high) / 2;

            //Check if the date at the middle index matches the date being searched for
            if (date == events[mid].GetDate())
            {
                //Check if the middle index is 0 or the date at the next index does not match the date being searched for
                if (mid == events.Count - 1 || events[mid + 1].GetDate() != date)
                {
                    return mid;
                }
                else
                {
                    //Continue searching in the upper half of the search range.
                    return BinarySearchLastIndex(events, date, mid + 1, high);
                }
            }
            else if (date < events[mid].GetDate())
            {
                //Continue searching in the lower half of the search range.
                return BinarySearchLastIndex(events, date, low, mid - 1);
            }
            else
            {
                //Continue searching in the upper half of the search range.
                return BinarySearchLastIndex(events, date, mid + 1, high);
            }
        }

        private void UserControlDay_Click(object sender, EventArgs e)
        {
            //Select the day
            SelectDay();
        }

        private void flpEvents_Click(object sender, EventArgs e)
        {
            //Select the day
            SelectDay();
        }

        //Pre: None
        //Select: None
        //Desc: Outlines and selects the day so calendar actions can be performed on it
        private void SelectDay()
        {
            //Check if the day is an actual day and not a blank for display reasons
            if (!isBlankDay)
            {
                //Check if there is already a selected day
                if (Form1.activeDay != null)
                {
                    //Remove the selection border from the previous active day
                    Form1.activeDay.BorderStyle = BorderStyle.None;
                }

                //Set this day as the new active day and draw a border around it
                BorderStyle = BorderStyle.FixedSingle;
                Form1.activeDay = this;
            }
        }

        private void lblMaxEvents_Click(object sender, EventArgs e)
        {
            //Select the day 
            SelectDay();

            //Create a list storing the remaining (hidden) events that aren't being displayed
            List<UserControlEvent> remainingEventsList = new List<UserControlEvent>();

            //Loop through all the events that aren't being displayed
            for (int i = events.Count - remainingEvents; i < events.Count; i++)
            {
                //Add them to the remaining events list
                remainingEventsList.Add(events[i]);
            }

            //Open the event edit form with the hidden events passed in
            EventEditForm eventEditForm = new EventEditForm(remainingEventsList);
            eventEditForm.ShowDialog();
        }

        private void lblMaxEvents_MouseHover(object sender, EventArgs e)
        {
            //Change cursor icon
            Cursor = Cursors.Hand;
            
            //Instantiate tool tip string
            string eventsInfo = "";

            //Loop through the reamining events
            for (int i = events.Count - remainingEvents; i < events.Count; i++)
            {
                //Check if the current event is the first remaining event
                if (i > events.Count - remainingEvents)
                {
                    //New line events info string
                    eventsInfo += "\n\n";
                }

                //Create and store DateTimes for the time start and end TimeSpans so AM/PM can be displayed
                DateTime timeStart = DateTime.Today.Add(events[i].GetTimeStart());
                DateTime timeEnd = DateTime.Today.Add(events[i].GetTimeEnd());

                //Add information to the events info string
                eventsInfo += "Title: " + events[i].GetEventName() + "\nStart Time: " + timeStart.ToString("hh:mm tt") + "\nEnd Time: " + timeEnd.ToString("hh:mm tt");
            }

            //Set the tooltip with the information
            ttMoreEvents.SetToolTip(lblMaxEvents, eventsInfo);
        }

        private void lblMaxEvents_MouseLeave(object sender, EventArgs e)
        {
            //Change mouse cursor icon
            Cursor = Cursors.Default;
        }
    }
}
