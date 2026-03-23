using Microsoft.EntityFrameworkCore;
using sylas_api.Database;
using sylas_api.Database.Models;

namespace sylas_api.JobManagers;

public class FamilyManager(SyContext context) : SyManager(context)
{
    #region FamilyMember

    public List<FamilyMember> FetchMembers()
    {
        return [.. _context.FamilyMembers.OrderBy(m => m.DisplayOrder)];
    }

    public FamilyMember FetchMember(long id)
    {
        return _context.FamilyMembers.Find(id)
            ?? throw new Exception($"Could not find family member {id}");
    }

    public FamilyMember CreateMember(string name, string? avatar, string? color, int displayOrder)
    {
        FamilyMember member = new()
        {
            Name = name,
            Avatar = avatar,
            Color = color,
            DisplayOrder = displayOrder,
            TotalPoints = 0,
            CreatedAt = DateTime.UtcNow
        };
        _context.FamilyMembers.Add(member);
        _context.SaveChanges();
        return member;
    }

    public FamilyMember UpdateMember(long id, string? name, string? avatar, string? color, int? displayOrder)
    {
        var member = FetchMember(id);
        if (name != null) member.Name = name;
        if (avatar != null) member.Avatar = avatar;
        if (color != null) member.Color = color;
        if (displayOrder != null) member.DisplayOrder = displayOrder.Value;
        _context.SaveChanges();
        return member;
    }

    public void DeleteMember(long id)
    {
        var member = FetchMember(id);
        _context.FamilyMembers.Remove(member);
        _context.SaveChanges();
    }

    #endregion

    #region FamilyTask

    private IQueryable<FamilyTask> GetTasks(DateTime? filterDate = null, long? memberId = null)
    {
        var day = (filterDate ?? DateTime.UtcNow.Date).Date;
        return _context.FamilyTasks
            .Include(t => t.Assignees)
            .Include(t => t.Completions!.Where(c =>
                c.CompletedDate.Date == day &&
                (memberId == null || c.MemberId == memberId)));
    }

    public List<FamilyTask> FetchTasks(long? memberId = null, FamilyTaskTimeOfDay? timeOfDay = null, DateTime? date = null)
    {
        var query = GetTasks(date, memberId)
            .Where(t => t.IsRecurring || t.Status != FamilyTaskStatus.Done);

        if (memberId != null)
            query = query.Where(t => t.Assignees!.Any(a => a.Id == memberId));

        if (timeOfDay != null)
            query = query.Where(t => t.TimeOfDay == timeOfDay || t.TimeOfDay == FamilyTaskTimeOfDay.AllDay);

        if (date != null)
        {
            var dayFlag = (int)MapDayOfWeek(date.Value.DayOfWeek);
            query = query.Where(t => !t.IsRecurring || ((int)t.RecurrenceDays & dayFlag) != 0);
        }

        return [.. query.OrderBy(t => t.DisplayOrder)];
    }

    private static FamilyRecurrenceDay MapDayOfWeek(DayOfWeek day) => day switch
    {
        DayOfWeek.Monday    => FamilyRecurrenceDay.Monday,
        DayOfWeek.Tuesday   => FamilyRecurrenceDay.Tuesday,
        DayOfWeek.Wednesday => FamilyRecurrenceDay.Wednesday,
        DayOfWeek.Thursday  => FamilyRecurrenceDay.Thursday,
        DayOfWeek.Friday    => FamilyRecurrenceDay.Friday,
        DayOfWeek.Saturday  => FamilyRecurrenceDay.Saturday,
        DayOfWeek.Sunday    => FamilyRecurrenceDay.Sunday,
        _                   => FamilyRecurrenceDay.None
    };

    public FamilyTask FetchTask(long id, DateTime? date = null, long? memberId = null)
    {
        return GetTasks(date, memberId).FirstOrDefault(t => t.Id == id)
            ?? throw new Exception($"Could not find family task {id}");
    }

    public FamilyTask CreateTask(string name, string? description, int pointsReward,
        FamilyTaskTimeOfDay timeOfDay, bool isRecurring, FamilyRecurrenceDay recurrenceDays,
        int displayOrder, DateTime? dueDate, List<long>? assigneeIds)
    {
        FamilyTask task = new()
        {
            Name = name,
            Description = description,
            PointsReward = pointsReward,
            TimeOfDay = timeOfDay,
            IsRecurring = isRecurring,
            RecurrenceDays = recurrenceDays,
            Status = FamilyTaskStatus.Active,
            DisplayOrder = displayOrder,
            DueDate = dueDate,
            CreatedAt = DateTime.UtcNow
        };
        if (assigneeIds != null && assigneeIds.Count > 0)
            task.Assignees = [.. _context.FamilyMembers.Where(m => assigneeIds.Contains(m.Id))];

        _context.FamilyTasks.Add(task);
        _context.SaveChanges();
        return FetchTask(task.Id);
    }

    public FamilyTask UpdateTask(long id, string? name, string? description, int? pointsReward,
        FamilyTaskTimeOfDay? timeOfDay, bool? isRecurring, FamilyRecurrenceDay? recurrenceDays,
        FamilyTaskStatus? status, int? displayOrder, DateTime? dueDate, List<long>? assigneeIds)
    {
        var task = FetchTask(id);
        if (name != null) task.Name = name;
        if (description != null) task.Description = description;
        if (pointsReward != null) task.PointsReward = pointsReward.Value;
        if (timeOfDay != null) task.TimeOfDay = timeOfDay.Value;
        if (isRecurring != null) task.IsRecurring = isRecurring.Value;
        if (recurrenceDays != null) task.RecurrenceDays = recurrenceDays.Value;
        if (status != null) task.Status = status.Value;
        if (displayOrder != null) task.DisplayOrder = displayOrder.Value;
        if (dueDate != null) task.DueDate = dueDate.Value;
        if (assigneeIds != null)
            task.Assignees = [.. _context.FamilyMembers.Where(m => assigneeIds.Contains(m.Id))];

        _context.SaveChanges();
        return FetchTask(id);
    }

    public void DeleteTask(long id)
    {
        var task = _context.FamilyTasks.Find(id)
            ?? throw new Exception($"Could not find family task {id}");
        _context.FamilyTasks.Remove(task);
        _context.SaveChanges();
    }

    #endregion

    #region Complete / Uncomplete

    public (FamilyTask task, FamilyMember member) CompleteTask(long taskId, long memberId)
    {
        var task = FetchTask(taskId);
        var member = FetchMember(memberId);

        var today = DateTime.UtcNow.Date;
        bool alreadyDone = _context.FamilyTaskCompletions
            .Any(c => c.TaskId == taskId && c.MemberId == memberId && c.CompletedDate.Date == today);

        if (alreadyDone)
            throw new Exception($"Task {taskId} already completed today by member {memberId}");

        FamilyTaskCompletion completion = new()
        {
            TaskId = taskId,
            MemberId = memberId,
            CompletedDate = DateTime.UtcNow,
            PointsEarned = task.PointsReward
        };
        _context.FamilyTaskCompletions.Add(completion);

        member.TotalPoints += task.PointsReward;

        if (!task.IsRecurring)
            task.Status = FamilyTaskStatus.Done;

        _context.SaveChanges();
        return (FetchTask(taskId, DateTime.UtcNow, memberId), FetchMember(memberId));
    }

    public (FamilyTask task, FamilyMember member) UncompleteTask(long taskId, long memberId)
    {
        var today = DateTime.UtcNow.Date;
        var completion = _context.FamilyTaskCompletions
            .FirstOrDefault(c => c.TaskId == taskId && c.MemberId == memberId && c.CompletedDate.Date == today)
            ?? throw new Exception($"No completion found for task {taskId} and member {memberId} today");

        var member = FetchMember(memberId);
        member.TotalPoints = Math.Max(0, member.TotalPoints - completion.PointsEarned);

        var task = _context.FamilyTasks.Find(taskId);
        if (task != null && task.Status == FamilyTaskStatus.Done)
            task.Status = FamilyTaskStatus.Active;

        _context.FamilyTaskCompletions.Remove(completion);
        _context.SaveChanges();
        return (FetchTask(taskId, DateTime.UtcNow, memberId), FetchMember(memberId));
    }

    #endregion
}
