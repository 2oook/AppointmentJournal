using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.Other
{
    public class DateTimePicker
    {
        public static List<DateTime[]> GetFourWeeks() 
        {
            var date = DateTime.Now;

            var daysList = new List<DateTime[]>();

            for (int i = 0; i < 4; i++)
            {
                var week = new DateTime[7];

                for (int index = 0; index < 7; index++)
                {
                    if (i == 0 && index == 0)
                    {
                        index = (int)DateTime.Now.DayOfWeek - 1;

                        if (index == -1) index += 7;  // воскресенье последний день недели
                    }

                    week[index] = date;
                    date = date.AddDays(1);
                }

                daysList.Add(week);
            }

            return daysList;
        }
    }
}
