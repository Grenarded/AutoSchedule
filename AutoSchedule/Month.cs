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
        private UserControlDay[] days;
        private int monthNum;
        private int year;
        private string monthName;

        FlowLayoutPanel flpDays;

        public Month(int monthNum, int year)
        {
            this.monthNum = monthNum;
            this.year = year;
            AssignMonthName();
            days = new UserControlDay[DateTime.DaysInMonth(year, monthNum)];

            //Create flow layout panel
            flpDays = new FlowLayoutPanel();

            flpDays.Location = new Point(32, 123);
            flpDays.Name = "flpDays";
            flpDays.Size = new Size(1059, 761);
            flpDays.TabIndex = 0;

            LoadDays();
        }

        public string GetMonthName()
        {
            return monthName;
        }

        private void AssignMonthName()
        {
            monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(monthNum);
        }

        private void LoadDays()
        {
            //Get first day of the month
            DateTime startofMonth = new DateTime(year, monthNum, 1);

            //Get num of days in the month 
            int numDays = DateTime.DaysInMonth(year, monthNum);

            int daysOfWeek = Convert.ToInt32(startofMonth.DayOfWeek.ToString("d"));

            for (int i = 0; i < daysOfWeek; i++)
            {
                UserControlBlank ucBlank = new UserControlBlank();
                flpDays.Controls.Add(ucBlank);
            }

            for (int i = 1; i <= numDays; i++)
            {
                UserControlDay ucDay = new UserControlDay(i);
                ucDay.DisplayDate();
                flpDays.Controls.Add(ucDay);

                //Check if the day user control is the current date's
                if (i == DateTime.Now.Day && monthNum == DateTime.Now.Month && year == DateTime.Now.Year)
                {
                    //Highlight box for the current date
                    ucDay.BackColor = Color.AntiqueWhite;
                }
            }
        }

        public FlowLayoutPanel DisplayDays()
        {
            return flpDays;
        }
    }
}
