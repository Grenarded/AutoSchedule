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
                //TODO: search main event list to find any events for this day
                //

                UserControlDay ucDay = new UserControlDay(i);
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

        public Control[] GetControls()
        {
            return controls;
        }
    }
}
