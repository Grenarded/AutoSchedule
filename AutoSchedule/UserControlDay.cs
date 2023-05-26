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

        private List<UserControlEvent> events;

        public UserControlDay(int dayNum)
        {
            this.dayNum = dayNum;
            InitializeComponent();
            AddEvents();
        }

        private void UserControlDay_Load(object sender, EventArgs e)
        {
        }

        private void AddEvents()
        {
            //TODO: add relevant events for each day 
            for (int i = 0; i < 1; i++)
            {
                UserControlEvent ucEvent = new UserControlEvent();
                flpEvents.Controls.Add(ucEvent);
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
