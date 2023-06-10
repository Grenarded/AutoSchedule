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

        //Event colours
        private readonly Color HEADER_COLOR = Color.LightBlue;
        private readonly Color EVENT_COLOR = Color.CornflowerBlue;

        //Maintain original column width
        private int originalColWidth; 

        private readonly DateTime START_TIME = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

        List<UserControlEvent> events;

        //track start and end rows (inner array) of each event (represented by each main array index)
        private int[][] eventRows;

        //track the event list index of the last event in each column
        private List<int> lastColEvent = new List<int>();

        private int numRows;

        //store day
        UserControlDay day;
        int curMonth;
        private int curYear;

        public ScheduleForm()
        {
            InitializeComponent();
        }

        public ScheduleForm(UserControlDay day, int curMonth, int curYear)
        {
            InitializeComponent();

            this.day = day;
            this.curMonth = curMonth;
            this.curYear = curYear;

            events = day.GetEvents();

            eventRows = new int[events.Count][];

            originalColWidth = dgvDay.Columns[0].Width;

            SetDateLabel();

            InstantiateRowTracker();
        }

        private void InstantiateRowTracker()
        {
            //Loop through each array within the array
            for (int i = 0; i < eventRows.Length; i++)
            {
                //Set nested array size to 2 (idx 0: start row, idx 1: end row)
                eventRows[i] = new int[2];
            }
        }

        private void ScheduleForm_Load(object sender, EventArgs e)
        {
            ttCalView.SetToolTip(btnCalView, "Calendar View");

            dgvDay.CellBorderStyle = DataGridViewCellBorderStyle.SingleVertical;

            numRows = DAY_HOURS * MINUTES_IN_HOUR / MINUTE_INTERVAL;
            LoadRows();
            SetCurTimeCell();

            CalcEventRowTimes();

            LayoutEventBlocks();
        }

        private void SetCurTimeCell()
        {
            //Scroll to row representing closest time to right now
            dgvDay.CurrentCell = dgvDay.Rows[(DateTime.Now.Hour * MINUTES_IN_HOUR + DateTime.Now.Minute) / MINUTE_INTERVAL].Cells[0];
        }

        private void LoadRows()
        {
            dgvDay.Rows.Add(numRows);

            DateTime time = START_TIME;
            //TODO: display tool tip for all row headers
            for (int i = 0; i < numRows; i += MINUTE_DISPLAY_INTERVAL / MINUTE_INTERVAL)
            {
                dgvDay.Rows[i].HeaderCell.Value = time.ToString("hh:mm tt");
                time = time.AddMinutes(MINUTE_DISPLAY_INTERVAL);
            }
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
                    DrawEventBlock(col, eventRows[i][START_ROW], eventRows[i][END_ROW], events[i].GetEventName(), i);
                }
                else
                {
                    lastColEvent.Add(col);
                    DrawEventBlock(col, eventRows[i][START_ROW], eventRows[i][END_ROW], events[i].GetEventName(), i);
                }
            }
        }

        private void DrawEventBlock(int col, int startRow, int endRow, string eventName, int eventIndex)
        {
            if (col > dgvDay.Columns.Count - 1) //NOTE: you should only ever need to add one column at a time
            {
                AddColumn();
            }

            for (int i = startRow; i <= endRow; i++)
            {
                dgvDay.Rows[i].Cells[col].Style.BackColor = EVENT_COLOR;
                dgvDay.Rows[i].Cells[col].Tag = eventIndex;

                if (i == startRow)
                {
                    dgvDay.Rows[i].Cells[col].Value = eventName;
                    dgvDay.Rows[i].Cells[col].Style.BackColor = HEADER_COLOR;
                }
            }
        }

        private void AddColumn()
        {
            DataGridViewColumn col = new DataGridViewColumn();
            col.CellTemplate = dgvDay.Columns[0].CellTemplate;

            //Set the new column width to be half the width of the first column
            col.Width = originalColWidth / (dgvDay.Columns.Count + 1);

            dgvDay.Columns.Add(col);

            //Resize columns' widths to match newest columns
            for (int i = 0; i < dgvDay.Columns.Count - 1; i++)
            {
                dgvDay.Columns[i].Width = col.Width;
            }
        }

        private void CalcEventRowTimes()
        {
            //Calculate and store the start row and end row value for each event
            for (int i = 0; i < eventRows.Length; i++)
            {
                eventRows[i][0] = ((events[i].GetTimeStart().Hours * MINUTES_IN_HOUR + events[i].GetTimeStart().Minutes) / MINUTE_INTERVAL);
                eventRows[i][1] = ((events[i].GetTimeEnd().Hours * MINUTES_IN_HOUR + events[i].GetTimeEnd().Minutes) / MINUTE_INTERVAL);
            }
        }

        private void ReloadSchedule()
        {
            SetDateLabel();
            events = day.GetEvents();
            eventRows = new int[events.Count][];

            InstantiateRowTracker();
            lastColEvent.Clear();

            CalcEventRowTimes();
            DataGridViewColumn col = dgvDay.Columns[0];
            col.Width = originalColWidth;
            dgvDay.Columns.Clear();
            dgvDay.Refresh();
            dgvDay.Columns.Add(col);
            LoadRows();
            LayoutEventBlocks();
            SetCurTimeCell();
        }

        //TODO: next and prev year
        private void btnNextDay_Click(object sender, EventArgs e)
        {
            if (day.GetDayNum() + 1 > DateTime.DaysInMonth(curYear, curMonth))
            {
                curMonth++;
                if (curMonth > Year.MONTHS_IN_YEAR)
                {
                    //TODO
                }
                day = Form1.year.GetMonth(curMonth).GetDay(1);
            }
            else
            {
                day = Form1.year.GetMonth(curMonth).GetDay(day.GetDayNum() + 1);
            }
            ReloadSchedule();
        }

        private void btnPrevDay_Click(object sender, EventArgs e)
        {
            if (day.GetDayNum() - 1 < 1)
            {
                curMonth--;
                if (curMonth > 0)
                {
                    //TODO
                }
                day = Form1.year.GetMonth(curMonth).GetDay(DateTime.DaysInMonth(curYear, curMonth));
            }
            else
            {
                day = Form1.year.GetMonth(curMonth).GetDay(day.GetDayNum() - 1);
            }
            ReloadSchedule();
        }

        private void SetDateLabel()
        {
            lblDate.Text = day.GetDayNum() + " " + Form1.year.GetMonth(curMonth).GetMonthName() + " " + curYear;
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

                    ttEventInfo.Show("Title: " + events[(int)activeCell.Tag].GetEventName()
                                    + "\nStart Time: " + events[(int)activeCell.Tag].GetTimeStart()
                                    + "\nEnd Time: " + events[(int)activeCell.Tag].GetTimeEnd(), 
                                    this, PointToClient(formLocation));
                }
            }
        }

        private void ScheduleForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void btnCalView_Click(object sender, EventArgs e)
        {
            Form form = new Form1();
            form.Location = Location;
            form.StartPosition = FormStartPosition.Manual;
            form.FormClosing += delegate { Close(); };
            form.Show();
            Hide();
        }
    }
}
