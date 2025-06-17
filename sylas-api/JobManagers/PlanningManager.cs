using System;
using Microsoft.EntityFrameworkCore;
using sylas_api.Database;
using sylas_api.Database.Models;
using sylas_api.Global;

namespace sylas_api.JobManagers;

public class PlanningManager(SyContext context) : SyManager(context)
{
    private IQueryable<PlanningItem> GeneratePlanningQuery() { return _context.PlanningItems.Include(p => p.User); }

    public List<PlanningItem> FetchPlanningForWeek(int weekNumber, int year)
    {
        (DateTime first, DateTime last) = Engine.GetFirstAndLastDayOfWeek(year, weekNumber);
        return [.. GeneratePlanningQuery().Where(p => p.PlannedDate >= first && p.PlannedDate <= last)];
    }

    public List<PlanningItem> FetchCurrentWeekPlanning()
    {
        return FetchPlanningForWeek(Engine.GetCurrentWeekNumber(), DateTime.Now.Year);
    }
}
