//Author: Ben Petlach
//File Name: Year.cs
//Project Name: AutoSchedule
//Creation Date: May 23, 2023
//Modified Date: June 6, 2023
//Description: Maintains the months in the year

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSchedule
{
    public class Year
    {
        //Store the number of months in a year
        public const int MONTHS_IN_YEAR = 12;

        //Store the month objects
        private Month[] months = new Month[MONTHS_IN_YEAR];

        //Store the year num
        private int year;

        public Year(int year)
        {
            this.year = year;

            //Create a new month object for each month in the year
            for (int i = 1; i <= MONTHS_IN_YEAR; i++)
            {
                //Add the month to the month list
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
