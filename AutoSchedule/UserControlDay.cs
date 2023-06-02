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

        private int recentEventIndex = 0;

        private List<UserControlEvent> events;

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

    private void UserControlDay_Load(object sender, EventArgs e)
        {
        }

        private void AddEvents()
        {
            //TODO: when reading in event times, round to the nearest 5 mins
            //TODO: add relevant events for each day 
            for (int i = recentEventIndex; i < events.Count; i++)
            {
                UserControlEvent ucEvent = new UserControlEvent(events[i].GetDate(), events[i].GetTimeStart(), events[i].GetTimeEnd(), events[i].GetEventName());
                flpEvents.Controls.Add(ucEvent);
                recentEventIndex = i + 1;
            }
            //for (int i = 0; i < 0; i++)
            //{
            //    UserControlEvent ucEvent = new UserControlEvent();
            //    flpEvents.Controls.Add(ucEvent);
            //}
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
