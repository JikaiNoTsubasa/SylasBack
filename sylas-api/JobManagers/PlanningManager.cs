using System;
using Microsoft.EntityFrameworkCore;
using sylas_api.Database;
using sylas_api.Database.Models;
using sylas_api.Exceptions;
using sylas_api.Global;

namespace sylas_api.JobManagers;

public class PlanningManager(SyContext context) : SyManager(context)
{
    private IQueryable<PlanningItem> GeneratePlanningQuery()
    {
        return _context.PlanningItems
            .Include(p => p.User)
            .Include(p => p.Owner)
            .Where(p => !p.IsDeleted);
    }

    public List<PlanningItem> FetchPlanningForWeek(int weekNumber, int year, long? loggedId = null)
    {
        (DateTime first, DateTime last) = Engine.GetFirstAndLastDayOfWeek(year, weekNumber);
        return [..
            GeneratePlanningQuery()
            .Where(p => p.PlannedDate >= first && p.PlannedDate <= last)
            .Where(loggedId is not null, p=> p.OwnerId == loggedId || p.Owner == null)
        ];
    }

    public List<PlanningItem> FetchCurrentWeekPlanning()
    {
        return FetchPlanningForWeek(Engine.GetCurrentWeekNumber(), DateTime.Now.Year);
    }

    public PlanningItem AddPlanningItem(string name, DateTime date, long loggedId, long? userId = null, string? description = null, bool? isPrivate = null)
    {
        var plan = new PlanningItem()
        {
            Name = name,
            Description = description,
            PlannedDate = date,
            UserId = userId
        };

        if (isPrivate != null && isPrivate.Value == true) plan.OwnerId = loggedId;

        plan.MarkCreated(loggedId);
        _context.PlanningItems.Add(plan);
        _context.SaveChanges();

        return plan;
    }

    public PlanningItem UpdatePlanningItem(long id, long loggedId, string? name = null, DateTime? date = null, long? userId = null, string? description = null, bool? isPrivate = null)
    {
        PlanningItem plan = _context.PlanningItems.Include(p => p.User).FirstOrDefault(p => p.Id == id) ?? throw new SyEntitiyNotFoundException($"Could not find planning item {id}");
        if (name != null) plan.Name = name;
        if (date != null) plan.PlannedDate = date.Value;
        if (userId != null) plan.UserId = userId.Value;
        if (description != null) plan.Description = description;
        if (isPrivate != null && isPrivate.Value == true) plan.OwnerId = loggedId;
        plan.MarkUpdated(loggedId);
        _context.SaveChanges();
        return plan;
    }
    
    public void DeletePlanningItem(long id, long deletedBy)
    {
        PlanningItem plan = _context.PlanningItems.FirstOrDefault(p => p.Id == id) ?? throw new SyEntitiyNotFoundException($"Could not find planning item {id}");
        plan.MarkDeleted(deletedBy);
        _context.SaveChanges();
    }
}
