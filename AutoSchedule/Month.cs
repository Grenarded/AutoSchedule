//Author: Ben Petlach
//File Name: Months.cs
//Project Name: AutoSchedule
//Creation Date: May 23, 2023
//Modified Date: June 12, 2023
//Description: Store and manage the collection of days associated with each month

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Drawing;
using System.Windows.Forms;

namespace AutoSchedule
{
    public class Month
    {
        //Maintain month-specific informatiom
        private int monthNum;
        private string monthName;

        private UserControlDay[] days;

        //Track year to get accurate month-day data
        private int year;

        //Track day info about the month (# of days, and # of days since Sunday until the first day of the month)
        private int daysInMonth;
        private int daysBeforeStart;

        public Month(int monthNum, int year)
        {
            this.monthNum = monthNum;
            this.year = year;
            AssignMonthName();

            //Store the number of days in the month
            daysInMonth = DateTime.DaysInMonth(year, monthNum);

            //Get first day of the month
            DateTime startOfMonth = new DateTime(year, monthNum, 1);
            daysBeforeStart = Convert.ToInt32(startOfMonth.DayOfWeek.ToString("d"));

            //Instantiate day array based on number of days in the month and number of days (from the first day of the week) until the first day
            days = new UserControlDay[daysBeforeStart + daysInMonth];

            LoadDays();
        }

        public string GetMonthName()
        {
            return monthName;
        }

        public Control[] GetDayControls()
        {
            return days;
        }

        public UserControlDay GetDay(int dayNum)
        {
            return days[daysBeforeStart + dayNum - 1];
        }

        //Pre: None
        //Post: None
        //Desc: Assign the month name depending on the month number
        private void AssignMonthName()
        {
            monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(monthNum);
        }

        //Pre: None
        //Post: None
        //Desc: Create and store all the days for the month, both blank (before start day) and actual (after start day)
        private void LoadDays()
        {
            //Loop through all the blank days
            for (int i = 0; i < daysBeforeStart; i++)
            {
                //Store blank day
                UserControlDay blankDay = new UserControlDay();
                days[i] = blankDay;
            }

            //Loop through actual days in the month
            for (int i = 1; i <= daysInMonth; i++)
            {
                UserControlDay ucDay = new UserControlDay();
                List<UserControlEvent> events = new List<UserControlEvent>();

                //Check if the day has any events associated with it
                if (ucDay.BinarySearchLastIndex(Form1.allEvents, new DateTime(year, monthNum, i), 0, Form1.allEvents.Count - 1) != -1)
                {
                    //Maintain first and last index of events on the main events list associated with this day 
                    int lastIndex = ucDay.BinarySearchLastIndex(Form1.allEvents, new DateTime(year, monthNum, i), 0, Form1.allEvents.Count - 1);
                    int firstIndex = ucDay.BinarySearchFirstIndex(Form1.allEvents, new DateTime(year, monthNum, i), 0, Form1.allEvents.Count - 1);

                    //Loop from the first event to last event (chronological time order)
                    while (firstIndex <= lastIndex)
                    {
                        //Add event to this day's event list
                        events.Add(Form1.allEvents[firstIndex]);
                        firstIndex++;
                    }

                    //Create day with event(s)
                    ucDay = new UserControlDay(i, events);
                }
                else
                {
                    //Create day without any events
                    ucDay = new UserControlDay(i);
                }

                //Display the date number
                ucDay.DisplayDate();

                //Store day object 
                days[i + daysBeforeStart - 1] = ucDay;

                //Check if the day user control is the current date's
                if (i == DateTime.Now.Day && monthNum == DateTime.Now.Month && year == DateTime.Now.Year)
                {
                    //Highlight box for the current date
                    ucDay.BackColor = Color.AntiqueWhite;
                }
            }
        }
    }
}
