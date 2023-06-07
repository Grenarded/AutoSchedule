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
    public partial class ScheduleForm : Form
    {
        private const int DAY_HOURS = 24;
        private const int MINUTES_IN_HOUR = 60;
        private const int MINUTE_INTERVAL = 5;
        private const int MINUTE_DISPLAY_INTERVAL = 15;

        private readonly DateTime START_TIME = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

        List<UserControlEvent> events;

        public ScheduleForm()
        {
            InitializeComponent();
        }

        public ScheduleForm(UserControlDay day)
        {
            InitializeComponent();
            events = day.GetEvents();
        }

        private void ScheduleForm_Load(object sender, EventArgs e)
        {
            dgvDay.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            int numRows = (DAY_HOURS * MINUTES_IN_HOUR / MINUTE_INTERVAL);
            
            DateTime time = START_TIME;

            dgvDay.Rows.Add(numRows);

            for (int i = 0; i < numRows; i += MINUTE_DISPLAY_INTERVAL / MINUTE_INTERVAL)
            {
                dgvDay.Rows[i].HeaderCell.Value = time.ToString("hh:mm tt");
                time = time.AddMinutes(MINUTE_DISPLAY_INTERVAL);
            }

            time = START_TIME;

            //TODO: recursive schedule filling?
            for (int i = 0; i < dgvDay.RowCount; i++)
            {
                for (int j = 0; j < events.Count; j++)
                {
                    {
                        TimeSpan headerTimeSpan = time.TimeOfDay;
                        if (headerTimeSpan >= events[j].GetTimeStart() && headerTimeSpan < events[j].GetTimeEnd())
                        {
                            dgvDay.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                        }
                    }
                }
                time = time.AddMinutes(MINUTE_INTERVAL);
            }
        }
    }
}
