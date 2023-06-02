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
    class Month
    {
        private UserControlDay[] days; //necessary w/ the Controls?
        private int monthNum;
        private int year;
        private string monthName;

        private Control[] controls;

        public Month(int monthNum, int year)
        {
            this.monthNum = monthNum;
            this.year = year;
            AssignMonthName();

            days = new UserControlDay[DateTime.DaysInMonth(year, monthNum)];

            LoadControls();
        }

        public string GetMonthName()
        {
            return monthName;
        }

        private void AssignMonthName()
        {
            monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(monthNum);
        }

        private void LoadControls()
        {
            //Get first day of the month
            DateTime startofMonth = new DateTime(year, monthNum, 1);

            //Get num of days in the month 
            int numDays = DateTime.DaysInMonth(year, monthNum);

            int daysOfWeek = Convert.ToInt32(startofMonth.DayOfWeek.ToString("d"));

            controls = new Control[daysOfWeek + numDays];

            for (int i = 0; i < daysOfWeek; i++)
            {
                UserControlBlank ucBlank = new UserControlBlank();
                controls[i] = ucBlank;
            }

            for (int i = 1; i <= numDays; i++)
            {
                UserControlDay ucDay;
                //TODO: search main event list to find any events for this day
                //
                //while (BinarySearch(Form1.allEvents, new DateTime(year, monthNum, i)) != null)
                {
                    //TODO: make it so that it doesnt return an event it already searched for
                }

                List<UserControlEvent> events = new List<UserControlEvent>();

                if (BinarySearchLastIndex(Form1.allEvents, new DateTime(year, monthNum, i), 0, Form1.allEvents.Count - 1) != -1)
                {
                    int lastIndex = BinarySearchLastIndex(Form1.allEvents, new DateTime(year, monthNum, i), 0, Form1.allEvents.Count - 1);
                    int firstIndex = BinarySearchFirstIndex(Form1.allEvents, new DateTime(year, monthNum, i), 0, Form1.allEvents.Count - 1);

                    while (lastIndex >= firstIndex)
                    {
                        events.Add(Form1.allEvents[lastIndex]);
                        lastIndex--;
                    }

                    ucDay = new UserControlDay(i, events);
                }
                else
                {
                    ucDay = new UserControlDay(i);
                }
                ucDay.DisplayDate();
                controls[i + daysOfWeek - 1] = ucDay;

                ////Check if the day user control is the current date's
                if (i == DateTime.Now.Day && monthNum == DateTime.Now.Month && year == DateTime.Now.Year)
                {
                    //Highlight box for the current date
                    ucDay.BackColor = Color.AntiqueWhite;
                }
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

        public static int BinarySearchLastIndex(List<UserControlEvent> events, DateTime date, int low, int high)
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

        public Control[] GetControls()
        {
            return controls;
        }
    }
}
