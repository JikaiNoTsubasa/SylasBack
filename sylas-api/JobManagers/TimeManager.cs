using System;
using Microsoft.AspNetCore.Authorization;
using sylas_api.Database;
using sylas_api.Database.Models;

namespace sylas_api.JobManagers;

public class TimeManager(SyContext context) : SyManager(context)
{
    public void AddTime(DateTime date, float minutes, long userId){
        DayTime dt = new(){
            Date = date,
            Minutes = minutes,
            UserId = userId
        };
        dt.MarkCreated(userId);
        _context.Times.Add(dt);
        _context.SaveChanges();
    }

    public List<DayTime> FetchMyLatestTimes(long userId){ 
        return [.. _context.Times.Where(t => t.UserId == userId).Take(10).OrderByDescending(t => t.Date)];
    }
}
