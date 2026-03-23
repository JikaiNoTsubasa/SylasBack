using System.ComponentModel.DataAnnotations;

namespace sylas_api.Database.Models;

public enum FamilyTaskTimeOfDay { AllDay, Morning, Noon, Evening }
public enum FamilyTaskStatus { Active, Done }

[Flags]
public enum FamilyRecurrenceDay
{
    None      = 0,
    Monday    = 1,
    Tuesday   = 2,
    Wednesday = 4,
    Thursday  = 8,
    Friday    = 16,
    Saturday  = 32,
    Sunday    = 64
}

public class FamilyTask
{
    [Key]
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int PointsReward { get; set; } = 1;
    public FamilyTaskTimeOfDay TimeOfDay { get; set; } = FamilyTaskTimeOfDay.AllDay;
    public bool IsRecurring { get; set; } = false;
    public FamilyRecurrenceDay RecurrenceDays { get; set; } = FamilyRecurrenceDay.None;
    public FamilyTaskStatus Status { get; set; } = FamilyTaskStatus.Active;
    public int DisplayOrder { get; set; } = 0;
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<FamilyMember>? Assignees { get; set; }
    public List<FamilyTaskCompletion>? Completions { get; set; }
}
