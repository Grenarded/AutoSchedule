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
        private const int MINUTE_INTERVAL = 15;
        private const int MINUTE_DISPLAY_INTERVAL = 60;

        //Track indices of start and end time in jagged array
        private const int START_ROW = 0;
        private const int END_ROW = 1;

        //Maintain original column width
        private int originalColWidth; 

        private readonly DateTime START_TIME = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

        List<UserControlEvent> events;

        //track start and end rows (inner array) of each event (represented by each main array index)
        private int[][] eventRows;

        //track the event list index of the last event in each column
        private List<int> lastColEvent = new List<int>();

        public ScheduleForm()
        {
            InitializeComponent();
        }

        public ScheduleForm(UserControlDay day)
        {
            InitializeComponent();
            events = day.GetEvents();

            eventRows = new int[events.Count][];

            originalColWidth = dgvDay.Columns[0].Width;

            //Loop through each array within the array
            for (int i = 0; i < eventRows.Length; i++)
            {
                //Set nested array size to 2 (idx 0: start row, idx 1: end row)
                eventRows[i] = new int[2];
            }
        }

        private void ScheduleForm_Load(object sender, EventArgs e)
        {
            dgvDay.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            int numRows = DAY_HOURS * MINUTES_IN_HOUR / MINUTE_INTERVAL;
            
            DateTime time = START_TIME;

            dgvDay.Rows.Add(numRows);

            time = START_TIME;
            //TODO: display tool tip for all row headers
            for (int i = 0; i < numRows; i += MINUTE_DISPLAY_INTERVAL / MINUTE_INTERVAL)
            {
                dgvDay.Rows[i].HeaderCell.Value = time.ToString("hh:mm tt");
                time = time.AddMinutes(MINUTE_DISPLAY_INTERVAL);
            }

            //Scroll to row representing closest time to right now
            dgvDay.CurrentCell = dgvDay.Rows[(DateTime.Now.Hour * MINUTES_IN_HOUR + DateTime.Now.Minute) / MINUTE_INTERVAL].Cells[0];
            
            //Calculate and store the start row and end row value for each event
            for (int i = 0; i < eventRows.Length; i++)
            {
                eventRows[i][0] = ((events[i].GetTimeStart().Hours * MINUTES_IN_HOUR + events[i].GetTimeStart().Minutes) / MINUTE_INTERVAL);
                eventRows[i][1] = ((events[i].GetTimeEnd().Hours * MINUTES_IN_HOUR + events[i].GetTimeEnd().Minutes) / MINUTE_INTERVAL);
                //MessageBox.Show("Start row: " + eventRows[i][0] + "\nEnd row: " + eventRows[i][1]);
            }

            LayoutEventBlocks();
        }

        private void LayoutEventBlocks()
        {
            for (int i = 0; i < events.Count; i++)
            {
                int col = 0;
                if (i > 0)
                {
                    col = -1;
                    for (int j = 0; j < lastColEvent.Count; j++)
                    {
                        //MessageBox.Show("" + lastColEvent[j] + "\n" + eventRows[lastColEvent[j]][START_ROW] + " >= " + eventRows[j][START_ROW] + " AND " + eventRows[i][START_ROW] + " <= " + eventRows[lastColEvent[j]][END_ROW]
                        //    + "\nOR\n" + eventRows[lastColEvent[j]][START_ROW] + " >= " + eventRows[i][START_ROW] + " AND " + eventRows[lastColEvent[j]][START_ROW] + " <= " + eventRows[i][END_ROW]);
                        if (!((eventRows[i][START_ROW] >= eventRows[lastColEvent[j]][START_ROW] && eventRows[i][START_ROW] <= eventRows[lastColEvent[j]][END_ROW])
                            || (eventRows[lastColEvent[j]][START_ROW] >= eventRows[i][START_ROW] && eventRows[lastColEvent[j]][START_ROW] <= eventRows[i][END_ROW])))
                        {
                            col = j;
                            lastColEvent[col] = i;
                            break;
                        }
                    }
                    if (col == -1)
                    {
                        col = lastColEvent.Count;
                        lastColEvent.Add(i);
                    }
                    DrawEventBlock(col, eventRows[i][START_ROW], eventRows[i][END_ROW], events[i].GetEventName());
                }
                else
                {
                    lastColEvent.Add(col);
                    DrawEventBlock(col, eventRows[i][START_ROW], eventRows[i][END_ROW], events[i].GetEventName());
                }
            }
        }

        private void DrawEventBlock(int col, int startRow, int endRow, string eventName)
        {
            if (col > dgvDay.Columns.Count - 1) //NOTE: you should only ever need to add one column at a time
            {
                AddColumn();
            }

            for (int i = startRow; i <= endRow; i++)
            {
                if (i == startRow)
                {
                    dgvDay.Rows[i].Cells[col].Value = eventName;
                }
                dgvDay.Rows[i].Cells[col].Style.BackColor = Color.CornflowerBlue;
            }
        }

        private void AddColumn()
        {
            DataGridViewColumn col = new DataGridViewColumn();
            col.CellTemplate = dgvDay.Columns[0].CellTemplate;
            //col.HeaderText = "Events";

            //Set the new column width to be half the width of the first column
            col.Width = originalColWidth / (dgvDay.Columns.Count + 1);
            //col.Width = 50;

            dgvDay.Columns.Add(col);

            //Resize columns' widths to match newest columns
            for (int i = 0; i < dgvDay.Columns.Count - 1; i++)
            {
                dgvDay.Columns[i].Width = col.Width;
            }

            //MessageBox.Show("" + dgvDay.Columns.Count);
        }
    }
}
