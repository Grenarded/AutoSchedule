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
        public const string EVENT_FILE = "Events.txt";

        //const int MONTHS_IN_YEAR = 12;
        public static int yearNum { get; private set; }
        public static int monthNum { get; private set; }

        private Year year;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            monthNum = DateTime.Now.Month;
            yearNum = DateTime.Now.Year;

            year = new Year(yearNum);

            DisplayDates();
        }

        private void DisplayDates()
        {
            //string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);

            lblMonthYear.Text = year.GetMonth(monthNum).GetMonthName() + " " + yearNum;

            flpDays = year.GetMonth(monthNum).DisplayDays();

            //Get first day of the month
            //DateTime startofMonth = new DateTime(year, month, 1);

            ////Get num of days in the month 
            //int numDays = DateTime.DaysInMonth(year, month);

            //int daysOfWeek = Convert.ToInt32(startofMonth.DayOfWeek.ToString("d"));

            //for (int i = 0; i < daysOfWeek; i++)
            //{
            //    UserControlBlank ucBlank = new UserControlBlank();
            //    flpDays.Controls.Add(ucBlank);
            //}

            //for (int i = 1; i <= numDays; i++)
            //{
            //    UserControlDay ucDay = new UserControlDay(i);
            //    ucDay.DisplayDate();
            //    flpDays.Controls.Add(ucDay);

            //    //Check if the day user control is the current date's
            //    if (i == DateTime.Now.Day && month == DateTime.Now.Month && year == DateTime.Now.Year)
            //    {
            //        //Highlight box for the current date
            //        ucDay.BackColor = Color.AntiqueWhite;
            //    }
            //}
        }

        private void btnNextMonth_Click(object sender, EventArgs e)
        {
            //Clear container
            flpDays.Controls.Clear();

            //check if year needs to be incrimented too 
            if (monthNum + 1 > Year.MONTHS_IN_YEAR)
            {
                yearNum++;
                monthNum = 0;
            }

            monthNum++;
            DisplayDates();
        }

        private void btnPrevMonth_Click(object sender, EventArgs e)
        {
            //Clear container
            flpDays.Controls.Clear();

            //check if year needs to be decreased too 
            if (monthNum - 1 < 1)
            {
                yearNum--;
                monthNum = Year.MONTHS_IN_YEAR + 1;
            }

            monthNum--;
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
