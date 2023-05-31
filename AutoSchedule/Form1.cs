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
        StreamWriter outFile;

        List<UserControlEvent> allEvents = new List<UserControlEvent>();

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

            ReadEvents();

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

            //FIX
            if (allEvents.Count > 0)
            {
                allEvents = MergeSort(allEvents.ToArray(), 0, allEvents.Count - 1).ToList();
            }
        }

        private UserControlEvent[] MergeSort(UserControlEvent[] events, int left, int right)
        {
            if (allEvents == null)
            {
                return null;
            }
            else if (left == right)
            {
                return new UserControlEvent[] { events[left] };
            }

            int mid = (left + right) / 2;

            return Merge(MergeSort(events, left, mid), MergeSort(events, mid + 1, right));
        }

        private UserControlEvent[] Merge(UserControlEvent[] left, UserControlEvent[] right)
        {
            //TODO: sort by time too

            //Base case 0: the left or right array has no elements
            if (left == null)
            {
                return right;
            }
            else if (right == null)
            {
                return left;
            }

            //Create a new array of size equal to the sum of the lengths of the left and right array
            UserControlEvent[] result = new UserControlEvent[left.Length + right.Length];

            //Ints pointing to the currently considered element of each given array
            int idx1 = 0;
            int idx2 = 0;

            //For each element in the merged array, get the next smallest element between the two given arrays
            for (int i = 0; i < result.Length; i++)
            {
                if (idx1 == left.Length)
                {
                    result[i] = right[idx2];
                    idx2++;
                }
                else if (idx2 == right.Length)
                {
                    result[i] = left[idx1];
                    idx1++;
                }
                else if (left[idx1].GetDate() < right[idx2].GetDate())
                {
                    result[i] = left[idx1];
                    idx1++;
                }
                else
                {
                    result[i] = right[idx2];
                }
            }
            return result;
        }

        //TODO: use to sort one event at a time when its added. Modify for this purpose
        private void InsertionSort(string[] data)
        {
            //Try in case file was messed with and indices are missing
            try
            {
                DateTime date = Convert.ToDateTime(data[0]);
                TimeSpan timeStart = TimeSpan.Parse(data[1]);
                TimeSpan timeEnd = TimeSpan.Parse(data[2]);
                string eventName = data[3];

                if (allEvents.Count > 1)
                {
                    //Insertion sort
                    for (int i = 1; i < allEvents.Count; i++)
                    {
                        if (allEvents[i].GetDate() < allEvents[i-1].GetDate())
                        {
                            for (int j = 1; j > 0; j--)
                            {
                                UserControlEvent temp = null;
                                if (allEvents[j].GetDate() < allEvents[j-1].GetDate())
                                {
                                    temp = allEvents[j - 1];
                                    allEvents[j - 1] = allEvents[j];
                                    allEvents[j] = temp;
                                }
                            }
                        }
                        else if (allEvents[i].GetDate() == allEvents[i-1].GetDate())
                        {
                            if (allEvents[i].GetTimeStart() < allEvents[i-1].GetTimeStart())
                            {
                                for (int j = i; j > 0; j--)
                                {
                                    UserControlEvent temp = null;
                                    if (allEvents[j].GetTimeStart() < allEvents[j-1].GetTimeStart())
                                    {
                                        temp = allEvents[j - 1];
                                        allEvents[j - 1] = allEvents[j];
                                        allEvents[j] = temp;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    allEvents.Add(new UserControlEvent(date, timeStart, timeEnd, eventName));
                }
            }
            catch
            {
                //TODO: popup?
            }

            /*
             * for (int i = 1; i < nums.Length; i++)
    {
      if (nums[i] < nums[i-1])
      {
        for (int j = i; j > 0; j--)
        {
          int temp = 0;
          if (nums[j] < nums[j-1])
          {
            temp = nums[j-1];
            nums[j-1] = nums[j];
            nums[j] = temp;
          }
        }
      }
    }*/
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                outFile = File.CreateText("sorted.txt");

                for (int i = 0; i < allEvents.Count; i++)
                {
                    outFile.WriteLine(allEvents[i].GetDate() + "," + allEvents[i].GetTimeStart() + "," + allEvents[i].GetTimeEnd() + "," + allEvents[i].GetEventName());
                }
            }
            catch
            {

            }
            finally
            {
                if (outFile != null)
                {
                    outFile.Close();
                }
            }
        }
        //
    }
}
