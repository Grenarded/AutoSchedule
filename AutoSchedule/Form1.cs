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

        StreamReader inFile;

        List<UserControlEvent> allEvents;

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

        private void ReadEvents()
        {
            try
            {
                string line;
                string[] data;

                inFile = File.OpenText(EVENT_FILE);

                while (!inFile.EndOfStream)
                {
                    line = inFile.ReadLine();
                    data = line.Split(',');


                }
            }
            catch
            {
                //TODO: popup?
            }
            finally
            {
                //Check if file was previously accessed
                if (inFile != null)
                {
                    inFile.Close();
                }
            }
        }

        private void DisplayDates()
        {
            flpDays.Controls.Clear();

            flpDays.Controls.AddRange(year.GetMonth(monthNum).GetControls()); //TODO: doesn't preload as expected, unless already been loaded before

            lblMonthYear.Text = year.GetMonth(monthNum).GetMonthName() + " " + yearNum;
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
