using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualBasic;
using sylas_api.Database;
using sylas_api.Database.Models;
using sylas_api.JobModels.TimeModel;

namespace sylas_api.JobManagers;

public class TimeManager(SyContext context) : SyManager(context)
{
    public void AddTime(DateTime date, float minutes, long userId){
        DayTime dt = new(){
            Name = "AddTime",
            Date = date,
            Minutes = minutes,
            UserId = userId
        };
        dt.MarkCreated(userId);
        _context.Times.Add(dt);
        _context.SaveChanges();
    }

    public List<DayTime> FetchMyLatestTimes(long userId){ 
        return [.. _context.Times.Where(t => t.UserId == userId).OrderByDescending(t => t.Date).Take(10)];
    }

    public ResponseMyTimeInfo GetMyTimeInfo(long userId){
        var times = _context.Times.Where(t => t.UserId == userId);

        DateTime currentDate = DateTime.Today;

        int monthTotal = (int)times.Where(t => t.Date.Year == currentDate.Year && t.Date.Month == currentDate.Month).Sum(t => t.Minutes);
        int total = (int)times.Sum(t => t.Minutes);

        return new(){
            TotalTimeBalance = total,
            MonthTimeBalance = monthTotal
        };
    }
}
