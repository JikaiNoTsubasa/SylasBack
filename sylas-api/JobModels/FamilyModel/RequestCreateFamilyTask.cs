using sylas_api.Database.Models;

namespace sylas_api.JobModels.FamilyModel;

public record RequestCreateFamilyTask
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int PointsReward { get; set; } = 1;
    public FamilyTaskTimeOfDay TimeOfDay { get; set; } = FamilyTaskTimeOfDay.AllDay;
    public bool IsRecurring { get; set; } = false;
    public FamilyRecurrenceDay RecurrenceDays { get; set; } = FamilyRecurrenceDay.None;
    public int DisplayOrder { get; set; } = 0;
    public DateTime? DueDate { get; set; }
    public List<long>? AssigneeIds { get; set; }
}
