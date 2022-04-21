using AppointmentJournal.AppDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.Other
{
    /// <summary>
    /// Класс для помощи в создании данных для выбора дат и времени
    /// </summary>
    public class DateTimePicker
    {
        // Метод для получения календаря из четырёх недель с информацией о доступности записи
        public static List<WorkDay[]> CreateFourWeeksCalendar(List<WorkDay> workDaysList) 
        {
            var date = DateTime.Now;

            var daysList = new List<WorkDay[]>();

            for (int i = 0; i < 4; i++)
            {
                var week = new WorkDay[7] 
                {
                    new WorkDay() { IsEnabled = false },
                    new WorkDay() { IsEnabled = false },
                    new WorkDay() { IsEnabled = false },
                    new WorkDay() { IsEnabled = false },
                    new WorkDay() { IsEnabled = false },
                    new WorkDay() { IsEnabled = false },
                    new WorkDay() { IsEnabled = false }
                };

                for (int index = 0; index < 7; index++)
                {
                    if (i == 0 && index == 0)
                    {
                        index = (int)DateTime.Now.DayOfWeek - 1;
                        if (index == -1) index += 7;  // воскресенье последний день недели
                    }

                    // получить дату, если она есть в списке рабочих дней
                    var dateInWorkDaysList = workDaysList.SingleOrDefault(x => x.Date.Date == date.Date);

                    if (dateInWorkDaysList != null && dateInWorkDaysList.Date.Date == date.Date)
                    {
                        week[index] = dateInWorkDaysList;
                    }
                    else
                    {
                        week[index].Date = date;
                    }

                    date = date.AddDays(1);
                }

                daysList.Add(week);
            }

            return daysList;
        }

        // Метод для получения промежутков времени для записи заданных производителем и разбитых по заданным порциям
        public static Dictionary<WorkDaysTimeSpan, List<AppointmentTime>> CreateAppointmentAvailableTimeList(int appointmentDurationInMinutes, List<WorkDaysTimeSpan> timeSpans)
        {
            var appointmentTimeByTimeSpansList = new Dictionary<WorkDaysTimeSpan,List<AppointmentTime>>();

            foreach (var timeSpan in timeSpans)
            {
                var appointmentTimeList = new List<AppointmentTime>();

                var appointmentsTimes = timeSpan.Appointments.Select(x => x.Time);

                var currentTime = timeSpan.BeginTime;

                while (currentTime < timeSpan.EndTime)
                {
                    var appointmentTime = new AppointmentTime()
                    {
                        Time = currentTime,
                        IsAvailable = false
                    };

                    if (appointmentsTimes.Contains(currentTime))
                    {
                        appointmentTime.IsAvailable = false;
                    }
                    else
                    {
                        appointmentTime.IsAvailable = true;
                    }

                    appointmentTimeList.Add(appointmentTime);

                    // инкремент даты на продолжительность сервиса в минутах
                    currentTime = currentTime.AddMinutes(appointmentDurationInMinutes);
                }

                appointmentTimeByTimeSpansList.Add(timeSpan, appointmentTimeList);
            }

            return appointmentTimeByTimeSpansList;
        }
    }

    /// <summary>
    /// Класс для представления времени бронирования
    /// </summary>
    public class AppointmentTime 
    {
        /// <summary>
        /// Время
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Флаг показывающий доступность бронирования
        /// </summary>
        public bool IsAvailable { get; set; }
    }
}
