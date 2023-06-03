using System;
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
            events.Add(newEvent);
            flpEvents.Controls.Add(newEvent);
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
            ShowEventForm();
        }

        private void flpEvents_Click(object sender, EventArgs e)
        {
            ShowEventForm();
        }

        private void ShowEventForm()
        {
            EventForm eventForm = new EventForm(new DateTime(Form1.yearNum, Form1.monthNum, dayNum));
            eventForm.ShowDialog();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel13_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
