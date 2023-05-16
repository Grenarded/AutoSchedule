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

namespace AutoSchedule
{
    public partial class Form1 : Form
    {
        const int MONTHS_IN_YEAR = 12;
        int year;
        int month;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            month = DateTime.Now.Month;
            year = DateTime.Now.Year;
            DisplayDates();
        }

        private void DisplayDates()
        {
            string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);

            lblMonthYear.Text = monthName + " " + year;
            //Get first day of the month
            DateTime startofMonth = new DateTime(year, month, 1);

            //Get num of days in the month 
            int numDays = DateTime.DaysInMonth(year, month);

            int daysOfWeek = Convert.ToInt32(startofMonth.DayOfWeek.ToString("d")) + 1;

            for (int i = 0; i < daysOfWeek; i++)
            {
                UserControlBlank ucBlank = new UserControlBlank();
                flpDays.Controls.Add(ucBlank);
            }

            for (int i = 1; i <= numDays; i++)
            {
                UserControlDay ucDay = new UserControlDay();
                ucDay.DisplayDate(i);
                flpDays.Controls.Add(ucDay);
            }
        }

        private void btnNextMonth_Click(object sender, EventArgs e)
        {
            //Clear container
            flpDays.Controls.Clear();

            //check if year needs to be incrimented too 
            if (month + 1 > MONTHS_IN_YEAR)
            {
                year++;
                month = 0;
            }

            month++;
            DisplayDates();
        }

        private void btnPrevMonth_Click(object sender, EventArgs e)
        {
            //Clear container
            flpDays.Controls.Clear();

            //check if year needs to be decreased too 
            if (month - 1 < 1)
            {
                year--;
                month = MONTHS_IN_YEAR + 1;
            }

            month--;
            DisplayDates();
        }

        //TODO: REMOVE
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
        //
    }
}
