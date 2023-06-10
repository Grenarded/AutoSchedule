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

        public static List<UserControlEvent> allEvents = new List<UserControlEvent>();

        public static int yearNum { get; private set; }
        public static int monthNum { get; private set; }
        
        public static Year year { get; private set; }

        public static UserControlDay activeDay;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            monthNum = DateTime.Now.Month;
            yearNum = DateTime.Now.Year;

            //Clear events if form is being loaded again (from schedule view)
            allEvents.Clear();

            ReadEvents();

            year = new Year(yearNum);

            //Set active day to the current day
            activeDay = year.GetMonth(monthNum).GetDay(DateTime.Now.Day);

            DisplayDates();

            //TODO: when hovering and they're disabled, change message saying that they must select a day
            ttAddEvent.SetToolTip(btnAddEvent, "Add Event");
            ttDailyView.SetToolTip(btnDailyView, "Daily View");
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

                    
                    DateTime date = Convert.ToDateTime(data[0]);
                    TimeSpan timeStart = TimeSpan.Parse(data[1]);
                    TimeSpan timeEnd = TimeSpan.Parse(data[2]);
                    string eventName = data[3];

                    allEvents.Add(new UserControlEvent(date, timeStart, timeEnd, eventName));
                    
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

            allEvents = MergeSort(allEvents);
        }

        private List<UserControlEvent> MergeSort(List<UserControlEvent> events)
        {
            if (events.Count <= 1)
            {
                return events;
            }

            List<UserControlEvent> left = new List<UserControlEvent>();
            List<UserControlEvent> right = new List<UserControlEvent>();

            int middle = events.Count / 2;

            for (int i = 0; i < middle; i++)
            {
                left.Add(events[i]);
            }
            for (int i = middle; i < events.Count; i++)
            {
                right.Add(events[i]);
            }

            left = MergeSort(left);
            right = MergeSort(right);
            return Merge(left, right);
        }

        private List<UserControlEvent> Merge(List<UserControlEvent> left, List<UserControlEvent> right)
        {
            List<UserControlEvent> result = new List<UserControlEvent>();

            while (left.Count > 0 || right.Count > 0)
            {
                if (left.Count > 0 && right.Count > 0)
                {
                    //if (left[0].GetDate() <= right[0].GetDate())
                    if (left[0].GetDate() < right[0].GetDate() || (left[0].GetDate() == right[0].GetDate() && left[0].GetTimeStart() < right[0].GetTimeStart()))
                    {
                        result.Add(left[0]);
                        left.Remove(left[0]);
                    }
                    else
                    {
                        result.Add(right[0]);
                        right.Remove(right[0]);
                    }
                }
                else if (left.Count > 0)
                {
                    result.Add(left[0]);
                    left.Remove(left[0]);
                }
                else if (right.Count > 0)
                {
                    result.Add(right[0]);
                    right.Remove(right[0]);
                }
            }
            return result;
        }

        public static void AddEvent(UserControlEvent newEvent)
        {
            UserControlDay day = year.GetMonth(newEvent.GetDate().Month).GetDay(newEvent.GetDate().Day);

            day.InsertionSort(allEvents, newEvent);
            day.AddEvent(newEvent);
        }

        public static void DeleteEvent(UserControlEvent deleteEvent)
        {
            UserControlDay day = year.GetMonth(deleteEvent.GetDate().Month).GetDay(deleteEvent.GetDate().Day);

            day.DeleteEvent(deleteEvent);
            allEvents.RemoveAt(day.SearchEvent(allEvents, deleteEvent));
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
                year = new Year(yearNum);
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
                year = new Year(yearNum);
            }

            monthNum--;
            DisplayDates();
        }

        private void ShowEventForm()
        {
            try
            {
                //Sometimes crashes
                if (activeDay != null)
                {
                    EventForm eventForm = new EventForm(new DateTime(yearNum, monthNum, activeDay.GetDayNum()));
                    eventForm.ShowDialog();
                }
            }
            catch
            {

            }
        }

        //TODO: REMOVE
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
        }
        //

        private void btnAddEvent_Click(object sender, EventArgs e)
        {
            ShowEventForm();
        }

        private void btnDailyView_Click(object sender, EventArgs e)
        {
            Form form = new ScheduleForm(activeDay, monthNum, yearNum);
            form.Location = Location;
            form.StartPosition = FormStartPosition.Manual;
            form.FormClosing += delegate { Close(); };
            form.Show();
            Hide();
        }
    }
}
