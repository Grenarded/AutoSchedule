//Author: Ben Petlach
//File Name: ScheduleForm.cs
//Project Name: AutoSchedule
//Creation Date: June 6, 2023
//Modified Date: June 12, 2023
//Description: Manage and display the daily view of all the events

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
        //Store necessary date and display information 
        private const int DAY_HOURS = 24;
        private const int MINUTES_IN_HOUR = 60;
        private const int MINUTE_INTERVAL = 15;
        private const int MINUTE_DISPLAY_INTERVAL = 60;

        //Track indices of start and end time in jagged array
        private const int START_ROW = 0;
        private const int END_ROW = 1;

        //Event colours
        private readonly Color HEADER_COLOR = Color.LightBlue;
        private readonly Color EVENT_COLOR = Color.CornflowerBlue;

        //Initial start time for beginning of each day
        private readonly DateTime START_TIME = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

        //Maintain all the events for the day 
        List<UserControlEvent> events;

        //Maintain original column width
        private int originalColWidth;

        //Track start and end rows (inner array) of each event (represented by each outer array index)
        private int[][] eventRows;

        //Track the local event list index of the last event in each column
        private List<int> lastColEvent = new List<int>();

        //Store number of rows generated depending on prior variables
        private int numRows;

        //Store day
        UserControlDay day;

        //Store year and month ints
        int curMonth;
        private int curYear;

        //Store parent form
        Form parentForm;

        public ScheduleForm()
        {
            InitializeComponent();
        }

        public ScheduleForm(UserControlDay day, int curMonth, int curYear, Form parentForm)
        {
            InitializeComponent();

            //Change visibility
            Visible = false;

            //Assign values from parameters
            this.parentForm = parentForm;
            this.day = day;
            this.curMonth = curMonth;
            this.curYear = curYear;

            //Update day's events
            events = day.GetEvents();

            //Instantiate array. Outer length is dependant on the number of events
            eventRows = new int[events.Count][];

            //Store original column width
            originalColWidth = dgvDay.Columns[0].Width;

            //Display the date label info
            SetDateLabel();

            //Instantiate the inner arrays of the eventRows array
            InstantiateRowTracker();
        }

        public void SetDay(UserControlDay day)
        {
            this.day = day;
        }

        public void SetMonth (int month)
        {
            curMonth = month;
        }

        public void SetYear (int year)
        {
            curYear = year;
        }

        private void SetDateLabel()
        {
            lblDate.Text = day.GetDayNum() + " " + Form1.year.GetMonth(curMonth).GetMonthName() + " " + curYear;
        }

        //Pre: None
        //Post: None
        //Desc: Instantiates inner arrays of eventRows
        private void InstantiateRowTracker()
        {
            //Loop through each array within the array
            for (int i = 0; i < eventRows.Length; i++)
            {
                //Set nested array size to 2 (idx 0: start row, idx 1: end row)
                eventRows[i] = new int[2];
            }
        }

        //Pre: None
        //Post: None
        //Highlight the cell representing the current time (within its 5 min range)
        private void SetCurTimeCell()
        {
            //Scroll to row representing closest time to right now
            dgvDay.CurrentCell = dgvDay.Rows[(DateTime.Now.Hour * MINUTES_IN_HOUR + DateTime.Now.Minute) / MINUTE_INTERVAL].Cells[0];
        }

        //Pre: None
        //Post: None
        //Desc: Add in and format all the rows
        private void LoadRows()
        {
            //Add all the rows
            dgvDay.Rows.Add(numRows);

            //Reset time
            DateTime time = START_TIME;

            //Loop through all the rows by the minute display interval / minute interval (causes each row to represent a range of time
            for (int i = 0; i < numRows; i += MINUTE_DISPLAY_INTERVAL / MINUTE_INTERVAL)
            {
                //Format and display time string on the row header
                dgvDay.Rows[i].HeaderCell.Value = time.ToString("hh:mm tt");
                time = time.AddMinutes(MINUTE_DISPLAY_INTERVAL);
            }
        }

        //Pre: None
        //Post: None
        //Desc: Calculate which rows and columns to draw an event block on
        private void LayoutEventBlocks()
        {
            //Loop through all the events
            for (int i = 0; i < events.Count; i++)
            {
                int col = 0;

                //Check if the event list has more than one event
                if (i > 0)
                {
                    //Set col to an invalid index
                    col = -1;

                    //Loop through the last event within each column
                    for (int j = 0; j < lastColEvent.Count; j++)
                    {
                        //Check if either the current event intersects with the previous event in the same column
                        if (!((eventRows[i][START_ROW] >= eventRows[lastColEvent[j]][START_ROW] && eventRows[i][START_ROW] <= eventRows[lastColEvent[j]][END_ROW])
                            || (eventRows[lastColEvent[j]][START_ROW] >= eventRows[i][START_ROW] && eventRows[lastColEvent[j]][START_ROW] <= eventRows[i][END_ROW])))
                        {
                            //Set the value of the new column such that the events don't intersect
                            col = j;
                            //Update the last last event index for that column
                            lastColEvent[col] = i;

                            //Break out of the loop
                            break;
                        }
                    }

                    //Check if col is still invalid (meaning no intersection occurs)
                    if (col == -1)
                    {
                        //Set the column index to a new column (one more than the current amount)
                        col = lastColEvent.Count;

                        //Add the index of the event added to that column
                        lastColEvent.Add(i);
                    }

                    //Draw the cells for the event
                    DrawEventBlock(col, eventRows[i][START_ROW], eventRows[i][END_ROW], events[i].GetEventName(), i);
                }
                else
                {
                    //Add and draw the first event's cells
                    lastColEvent.Add(col);
                    DrawEventBlock(col, eventRows[i][START_ROW], eventRows[i][END_ROW], events[i].GetEventName(), i);
                }
            }
        }

        //Pre: ints representing the column index, start and end indices for the rows representing the event, the event name as a string, and the event's index
        //Post: None
        //Desc: Highlight the cells representing the event
        private void DrawEventBlock(int col, int startRow, int endRow, string eventName, int eventIndex)
        {
            //Check if the value of columns is greater than the amount there currently is
            if (col > dgvDay.Columns.Count - 1) 
            {
                //Add a new column
                AddColumn();
            }

            //Loop from the event's start row to end row
            for (int i = startRow; i <= endRow; i++)
            {
                //Colour in the cells in the corresponding row and column
                dgvDay.Rows[i].Cells[col].Style.BackColor = EVENT_COLOR;

                //Tag the cells with the event index
                dgvDay.Rows[i].Cells[col].Tag = eventIndex;

                //Check if the current row is the start row
                if (i == startRow)
                {
                    //Colour in the cells in the corresponding row and column as well as write the name of the event
                    dgvDay.Rows[i].Cells[col].Value = eventName;
                    dgvDay.Rows[i].Cells[col].Style.BackColor = HEADER_COLOR;
                }
            }
        }

        //Pre: None
        //Post: None
        //Desc: Add's a column to the grid view
        private void AddColumn()
        {
            //Instantiate a new column
            DataGridViewColumn col = new DataGridViewColumn();
            col.CellTemplate = dgvDay.Columns[0].CellTemplate;

            //Set the new column width to be half the width of the first column
            col.Width = originalColWidth / (dgvDay.Columns.Count + 1);

            //Add column
            dgvDay.Columns.Add(col);

            //Loop through all the columns 
            for (int i = 0; i < dgvDay.Columns.Count - 1; i++)
            {
                //Resize columns' widths to match newest columns
                dgvDay.Columns[i].Width = col.Width;
            }
        }

        //Pre: None
        //Post: None
        //Desc: Calculate the times start and end rows represent
        private void CalcEventRowTimes()
        {
            //Loop through all the rows
            for (int i = 0; i < eventRows.Length; i++)
            {
                //Calculate and store the start row and end row value for each event's start and end time
                eventRows[i][0] = ((events[i].GetTimeStart().Hours * MINUTES_IN_HOUR + events[i].GetTimeStart().Minutes) / MINUTE_INTERVAL);
                eventRows[i][1] = ((events[i].GetTimeEnd().Hours * MINUTES_IN_HOUR + events[i].GetTimeEnd().Minutes) / MINUTE_INTERVAL);
            }
        }

        //Pre: None
        //Post: None
        //Desc: Updates values and redraws controls 
        public void ReloadSchedule()
        {
            //update date lable
            SetDateLabel();

            //Update the day's events and corresponding eventRows array
            events = day.GetEvents();
            eventRows = new int[events.Count][];

            //Instantiate eventRows array
            InstantiateRowTracker();

            //Clear list tracking indices of the last event in each column
            lastColEvent.Clear();

            //Calculate the rows representing end and start times
            CalcEventRowTimes();

            //Restart the current col
            DataGridViewColumn col = dgvDay.Columns[0];
            col.Width = originalColWidth;

            //Restart the datagridview
            dgvDay.Columns.Clear();
            dgvDay.Refresh();
            dgvDay.Columns.Add(col);

            //Clear out and re-add rows with corresponding events
            LoadRows();
            LayoutEventBlocks();

            //Update the current time
            SetCurTimeCell();
        }

        private void ScheduleForm_Load(object sender, EventArgs e)
        {
            //Calculate the number of rows needed
            numRows = DAY_HOURS * MINUTES_IN_HOUR / MINUTE_INTERVAL;

            //Set the tooltip
            ttCalView.SetToolTip(btnCalView, "Calendar View");

            //Add vetical border between columns
            dgvDay.CellBorderStyle = DataGridViewCellBorderStyle.SingleVertical;

            //Load and create the necessary rows
            LoadRows();

            //Highlight the cell representing the current time
            SetCurTimeCell();

            //Calculate the rows representing end and start times
            CalcEventRowTimes();

            //Layout the blocks for each event
            LayoutEventBlocks();
        }

        private void btnNextDay_Click(object sender, EventArgs e)
        {
            //Check if the next day is outside the month
            if (day.GetDayNum() + 1 > DateTime.DaysInMonth(curYear, curMonth))
            {
                //Check if the next month is within the year
                if (curMonth + 1 < Year.MONTHS_IN_YEAR)
                {
                    //Update the month and day
                    curMonth++;
                    day = Form1.year.GetMonth(curMonth).GetDay(1);
                }
            }
            else
            {
                //Update the day
                day = Form1.year.GetMonth(curMonth).GetDay(day.GetDayNum() + 1);
            }

            //Reload the schedule
            ReloadSchedule();
        }

        private void btnPrevDay_Click(object sender, EventArgs e)
        {
            //Check if the day is outside the month
            if (day.GetDayNum() - 1 < 1)
            {
                //Check if the month is within the year
                if (curMonth - 1 >= 0)
                {
                    //Update month and day
                    curMonth--;
                    day = Form1.year.GetMonth(curMonth).GetDay(DateTime.DaysInMonth(curYear, curMonth));
                }
            }
            else
            {
                //Update the day
                day = Form1.year.GetMonth(curMonth).GetDay(day.GetDayNum() - 1);
            }

            //Reload the schedule
            ReloadSchedule();
        }

        
        private void dgvDay_MouseHover(object sender, EventArgs e)
        {

        }
        

        private void dgvDay_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell activeCell = dgvDay.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (activeCell.Style.BackColor == HEADER_COLOR || activeCell.Style.BackColor == EVENT_COLOR)
                {
                    // Get mouse position on the form
                    Point formLocation = Control.MousePosition;

                    //Get DateTime values of time spans so that they can be formatted into 12-hour format
                    DateTime timeStart = DateTime.Today.Add(events[(int)activeCell.Tag].GetTimeStart());
                    DateTime timeEnd = DateTime.Today.Add(events[(int)activeCell.Tag].GetTimeEnd());

                    //Display tool tip with event information
                    ttEventInfo.Show("Title: " + events[(int)activeCell.Tag].GetEventName()
                                    + "\nStart Time: " + timeStart.ToString("hh:mm tt")
                                    + "\nEnd Time: " + timeEnd.ToString("hh:mm tt"),
                                    this, PointToClient(formLocation));
                }
            }
        }

        private void ScheduleForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Close the parent form, closing the program
            parentForm.Close();
        }

        private void btnCalView_Click(object sender, EventArgs e)
        {
            //Update location of the parent form
            parentForm.Location = Location;
            parentForm.StartPosition = FormStartPosition.Manual;

            //Swap form visibility
            parentForm.Visible = true;
            Visible = false;
        }
    }
}
