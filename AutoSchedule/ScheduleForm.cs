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

        //Track indices of start and end time in jagged array
        private const int START_ROW = 0;
        private const int END_ROW = 1;

        //Maintain original column width
        private int originalColWidth; 

        private readonly DateTime START_TIME = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

        List<UserControlEvent> events;

        private int[][] eventRows;

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
            
            //Calculate and store the start row and end row value for each event
            for (int i = 0; i < eventRows.Length; i++)
            {
                eventRows[i][0] = ((events[i].GetTimeStart().Hours * MINUTES_IN_HOUR + events[i].GetTimeStart().Minutes) / MINUTE_INTERVAL);
                eventRows[i][1] = ((events[i].GetTimeEnd().Hours * MINUTES_IN_HOUR + events[i].GetTimeEnd().Minutes) / MINUTE_INTERVAL);
                //MessageBox.Show("Start row: " + eventRows[i][0] + "\nEnd row: " + eventRows[i][1]);
            }

            LayoutEventBlocks();

            /*
             ALGORITHM IDEA:
            -An array of int lists. 
                -Each array element represents an event
                -Each list contains the row int of the timespan
            -Loop through the array, populating each list
            -Sort by start time
            -Check
                -if start time of first == start time of second
                -if end time of first < end time of second
                -if end time of second < end time of first
                -IF any of these conditions are met, create new column for the second element
            -Check
                -third against the first
                -Same criteria as above
                -IF any conditions are met, compare against the second
                -IF any conditions are met, create new column
            -Continue like this
             */

            //Loop through each event TimeSpan, tracking the start and end columns

            //time = START_TIME;
            //for (int i = 0; i < dgvDay.RowCount; i++)
            //{
            //    for (int j = 0; j < events.Count; j++)
            //    {
            //        {
            //            TimeSpan headerTimeSpan = time.TimeOfDay;
            //            if (headerTimeSpan >= events[j].GetTimeStart() && headerTimeSpan < events[j].GetTimeEnd())
            //            {
            //                dgvDay.Rows[i].DefaultCellStyle.BackColor = Color.Green;
            //            }
            //        }
            //    }
            //    time = time.AddMinutes(MINUTE_INTERVAL);
            //}
        }

        //DOESN'T WORK
        private void LayoutEventBlocks()
        {
            int maxCol;//= dgvDay.Columns.Count;
            for (int i = 0; i < events.Count; i++)
            {
                maxCol = dgvDay.Columns.Count; 

                int col = 0;
                if (i > 0)
                {
                    /*
                    for (int j = 0; j < i; j++)
                    {
                        if ((eventRows[i][START_ROW] >= eventRows[j][START_ROW] && eventRows[i][START_ROW] <= eventRows[j][END_ROW])
                            || (eventRows[j][START_ROW] >= eventRows[i][START_ROW] && eventRows[j][START_ROW] <= eventRows[i][END_ROW]))
                        {
                            col++; 
                        }
                        else
                        {
                            col = j;
                            //Check 
                            //break;
                        }
                    }
                    */
                    //TODO: FIX
                        //Idea: Work the way backwards. Keep track of which columns work and which don't. The last column to work is the column to set it to
                    col = maxCol;
                    int prevGoodCol = col;
                    for (int j = i; j > 0; j--)
                    {
                        j--;
                        if (!(eventRows[i][START_ROW] >= eventRows[j][START_ROW] && eventRows[i][START_ROW] <= eventRows[j][END_ROW])
                            || !(eventRows[j][START_ROW] >= eventRows[i][START_ROW] && eventRows[j][START_ROW] <= eventRows[i][END_ROW]))
                        {
                            col--;
                            prevGoodCol = col;
                        }
                        else
                        {
                            col--;
                        }
                    }
                    DrawEventBlock(prevGoodCol, eventRows[i][START_ROW], eventRows[i][END_ROW]);
                    

                    /*
                    for (int j = i-1; j > 0; j--)
                    {
                        if ((eventRows[i][START_ROW] >= eventRows[j][START_ROW] && eventRows[i][START_ROW] <= eventRows[j][END_ROW])
                            || (eventRows[j][START_ROW] >= eventRows[i][START_ROW] && eventRows[j][START_ROW] <= eventRows[i][END_ROW]))
                        {

                        }
                    }
                    */
                }
                else
                {
                    DrawEventBlock(col, eventRows[i][START_ROW], eventRows[i][END_ROW]);
                    //AddColumn();
                }
            }
        }

        private void DrawEventBlock(int col, int startRow, int endRow)
        {
            if (col > dgvDay.Columns.Count - 1) //NOTE: you should only ever need to add one column at a time
            {
                AddColumn();
            }

            for (int i = startRow; i <= endRow; i++)
            {
                dgvDay.Rows[i].Cells[col].Style.BackColor = Color.Green;
            }
        }

        private void AddColumn()
        {
            DataGridViewColumn col = new DataGridViewColumn();
            col.CellTemplate = dgvDay.Columns[0].CellTemplate;
            col.HeaderText = "Events";

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
