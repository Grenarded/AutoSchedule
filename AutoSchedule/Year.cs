using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSchedule
{
    public class Year
    {
        public const int MONTHS_IN_YEAR = 12;

        private Month[] months = new Month[12];

        private int year;

        public Year(int year)
        {
            this.year = year;
            for (int i = 1; i <= MONTHS_IN_YEAR; i++)
            {
                months[i - 1] = new Month(i, year);
            }
        }

        public Month GetMonth(int month)
        {
            return months[month-1];
        }

        public int GetYear()
        {
            return year;
        }
    }
}
