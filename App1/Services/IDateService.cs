using App1.Models;
using System;
using System.Collections.Generic;


namespace App1.Services
{
    public interface IDateService
    {
        WeekModel GetWeek(DateTime date);

        List<DayModel> GetDayList(DateTime firstDayInWeek, DateTime lastDayInWeek);
    }
}