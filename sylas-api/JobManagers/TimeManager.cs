using System;
using Microsoft.EntityFrameworkCore;
using sylas_api.Database;
using sylas_api.Database.Models;
using sylas_api.Exceptions;
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
        User user = _context.Users.Include(u => u.Preferences).FirstOrDefault(u => u.Id == userId) ?? throw new SyEntitiyNotFoundException($"User {userId} not found");
        return [.. _context.Times.Where(t => t.UserId == userId).OrderByDescending(t => t.Date).Take(user.Preferences?.TimeHistory ?? 10)];
    }

    public ResponseMyTimeInfo GetMyTimeInfo(long userId){
        User user = _context.Users.Include(u => u.Preferences).FirstOrDefault(u => u.Id == userId) ?? throw new SyEntitiyNotFoundException($"User {userId} not found");
        var times = _context.Times.Where(t => t.UserId == userId);

        DateTime currentDate = DateTime.Today;

        int totalEntries = times.Count();
        int monthTotal = (int)times.Where(t => t.Date.Year == currentDate.Year && t.Date.Month == currentDate.Month).Sum(t => t.Minutes);
        int total = (int)times.Sum(t => t.Minutes);
        float average = !times.Any() ? 0 : times.Average(t => t.Minutes);

        // Get total by month
        List<ResponseTotalByMonth> totalByMonth = [.. times
            .GroupBy(t => new { t.Date.Year, t.Date.Month })
            .Select(g => new ResponseTotalByMonth { Year = g.Key.Year, Month = g.Key.Month, Total = (int)g.Sum(t => t.Minutes) })
            .OrderBy(g => g.Year).ThenBy(g => g.Month)
            .Take(user.Preferences?.TimeChartMonth ?? 12)];

        return new(){
            TotalTimeBalance = total,
            TotalEntries = totalEntries,
            MoyBalance = average,
            MonthTimeBalance = monthTotal,
            TotalByMonth = totalByMonth
        };
    }
}
