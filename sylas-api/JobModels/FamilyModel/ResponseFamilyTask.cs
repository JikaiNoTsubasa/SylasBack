using sylas_api.Database.Models;

namespace sylas_api.JobModels.FamilyModel;

public record ResponseFamilyTask
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int PointsReward { get; set; }
    public FamilyTaskTimeOfDay TimeOfDay { get; set; }
    public bool IsRecurring { get; set; }
    public FamilyRecurrenceDay RecurrenceDays { get; set; }
    public FamilyTaskStatus Status { get; set; }
    public bool IsDoneToday { get; set; }
    public int DisplayOrder { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<ResponseFamilyMember>? Assignees { get; set; }
}
