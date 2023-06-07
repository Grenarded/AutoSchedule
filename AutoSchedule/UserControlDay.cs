﻿using System;
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
        private int dayNum;

        private List<UserControlEvent> events = new List<UserControlEvent>();

        public UserControlDay(int dayNum, List<UserControlEvent> events)
        {
            this.dayNum = dayNum;
            this.events = events;

            InitializeComponent();
            AddEvents();
        }

        public UserControlDay(int dayNum)
        {
            this.dayNum = dayNum;

            InitializeComponent();
        }

        public UserControlDay()
        {
            InitializeComponent();

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

        public void InsertionSort(List<UserControlEvent> eventList, UserControlEvent addedEvent)
        {
            //Try in case file was messed with and indices are missing
            try
            {
                eventList.Add(addedEvent);

                if (eventList.Count > 1)
                {
                    for (int i = 1; i < eventList.Count; i++)
                    {
                        int j = i;
                        while (j > 0 && eventList[j].GetDateAndTimeStart() < eventList[j - 1].GetDateAndTimeStart())
                        {
                            UserControlEvent temp = eventList[j];
                            eventList[j] = eventList[j - 1];
                            eventList[j - 1] = temp;
                            j--;
                        }
                    }
                }
            }
            catch
            {
                //TODO: popup?
            }
        }

        private void UserControlDay_Load(object sender, EventArgs e)
        {
        }

        private void AddEvents()
        {
            //TODO: when reading in event times, round to the nearest 5 mins
            for (int i = 0; i < events.Count; i++)
            {
                UserControlEvent ucEvent = new UserControlEvent(events[i].GetDate(), events[i].GetTimeStart(), events[i].GetTimeEnd(), events[i].GetEventName());
                flpEvents.Controls.Add(ucEvent);
            }
        }

        public void AddEvent(UserControlEvent newEvent)
        {
            InsertionSort(events, newEvent);
            flpEvents.Controls.Clear();
            AddEvents();
        }

        public void DeleteEvent(UserControlEvent deleteEvent)
        {
            events.RemoveAt(BinarySearchSpecific(events, deleteEvent.GetDateAndTimeStart(), 0, events.Count));
            flpEvents.Controls.Clear();
            AddEvents();
        }

        public int BinarySearchSpecific(List<UserControlEvent> events, DateTime dateTimeStart, int low, int high)
        {
            if (low > high)
            {
                return -1;
            }

            int mid = (low + high) / 2;

            if (dateTimeStart == events[mid].GetDateAndTimeStart())
            {
                if (mid == 0 || events[mid - 1].GetDateAndTimeStart() != dateTimeStart)
                {
                    return mid;
                }
                else
                {
                    return BinarySearchSpecific(events, dateTimeStart, low, mid - 1);
                }
            }
            else if (dateTimeStart < events[mid].GetDateAndTimeStart())
            {
                return BinarySearchSpecific(events, dateTimeStart, low, mid - 1);
            }
            else
            {
                return BinarySearchSpecific(events, dateTimeStart, mid + 1, high);
            }
        }

        public int BinarySearchFirstIndex(List<UserControlEvent> events, DateTime date, int low, int high)
        {
            if (low > high)
            {
                return -1;
            }

            int mid = (low + high) / 2;

            if (date == events[mid].GetDate())
            {
                if (mid == 0 || events[mid - 1].GetDate() != date)
                {
                    return mid;
                }
                else
                {
                    return BinarySearchFirstIndex(events, date, low, mid - 1);
                }
            }
            else if (date < events[mid].GetDate())
            {
                return BinarySearchFirstIndex(events, date, low, mid - 1);
            }
            else
            {
                return BinarySearchFirstIndex(events, date, mid + 1, high);
            }
        }

        public int BinarySearchLastIndex(List<UserControlEvent> events, DateTime date, int low, int high)
        {
            if (low > high)
            {
                return -1;
            }

            int mid = (low + high) / 2;

            if (date == events[mid].GetDate())
            {
                if (mid == events.Count - 1 || events[mid + 1].GetDate() != date)
                {
                    return mid;
                }
                else
                {
                    return BinarySearchLastIndex(events, date, mid + 1, high);
                }
            }
            else if (date < events[mid].GetDate())
            {
                return BinarySearchLastIndex(events, date, low, mid - 1);
            }
            else
            {
                return BinarySearchLastIndex(events, date, mid + 1, high);
            }
        }

        public void DisplayDate()
        {
            lblDayNum.Text = Convert.ToString(dayNum);
        }

        private void UserControlDay_Click(object sender, EventArgs e)
        {
            HighlightDay();
        }

        private void flpEvents_Click(object sender, EventArgs e)
        {
            try
            {
                HighlightDay();
            }
            catch
            {
                //
            }
        }

        private void HighlightDay()
        {
            if (Form1.activeDay != null)
            {
                Form1.activeDay.BorderStyle = BorderStyle.None;
            }

            BorderStyle = BorderStyle.FixedSingle;
            Form1.activeDay = this;
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel13_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
