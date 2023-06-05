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
       // private UserControlDay[] days; //necessary w/ the Controls?
        private int monthNum;
        private int year;
        private string monthName;

        private UserControlDay[] days;

        private int daysInMonth;
        private int daysBeforeStart;

        public Month(int monthNum, int year)
        {
            this.monthNum = monthNum;
            this.year = year;
            AssignMonthName();

            daysInMonth = DateTime.DaysInMonth(year, monthNum);

            //Get first day of the month
            DateTime startOfMonth = new DateTime(year, monthNum, 1);

            daysBeforeStart = Convert.ToInt32(startOfMonth.DayOfWeek.ToString("d"));

            days = new UserControlDay[daysBeforeStart + daysInMonth];

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

        public UserControlDay GetDay(int dayNum)
        {
            return days[daysBeforeStart + dayNum - 1];
        }

        private void LoadControls()
        {
            for (int i = 0; i < daysBeforeStart; i++)
            {
                //UserControlBlank ucBlank = new UserControlBlank();
                UserControlDay blankDay = new UserControlDay();
                days[i] = blankDay;
            }

            for (int i = 1; i <= daysInMonth; i++)
            {
                UserControlDay ucDay = new UserControlDay();

                List<UserControlEvent> events = new List<UserControlEvent>();

                if (ucDay.BinarySearchLastIndex(Form1.allEvents, new DateTime(year, monthNum, i), 0, Form1.allEvents.Count - 1) != -1)
                {
                    int lastIndex = ucDay.BinarySearchLastIndex(Form1.allEvents, new DateTime(year, monthNum, i), 0, Form1.allEvents.Count - 1);
                    int firstIndex = ucDay.BinarySearchFirstIndex(Form1.allEvents, new DateTime(year, monthNum, i), 0, Form1.allEvents.Count - 1);

                    while (firstIndex <= lastIndex)
                    {
                        events.Add(Form1.allEvents[firstIndex]);
                        firstIndex++;
                    }

                    ucDay = new UserControlDay(i, events);
                }
                else
                {
                    ucDay = new UserControlDay(i);
                }

                ucDay.DisplayDate();
                days[i + daysBeforeStart - 1] = ucDay;

                //Check if the day user control is the current date's
                if (i == DateTime.Now.Day && monthNum == DateTime.Now.Month && year == DateTime.Now.Year)
                {
                    //Highlight box for the current date
                    ucDay.BackColor = Color.AntiqueWhite;
                }
            }
        }

        public Control[] GetControls()
        {
            return days;
        }
    }
}
