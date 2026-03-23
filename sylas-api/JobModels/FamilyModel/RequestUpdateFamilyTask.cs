using sylas_api.Database.Models;

namespace sylas_api.JobModels.FamilyModel;

public record RequestUpdateFamilyTask
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? PointsReward { get; set; }
    public FamilyTaskTimeOfDay? TimeOfDay { get; set; }
    public bool? IsRecurring { get; set; }
    public FamilyRecurrenceDay? RecurrenceDays { get; set; }
    public FamilyTaskStatus? Status { get; set; }
    public int? DisplayOrder { get; set; }
    public DateTime? DueDate { get; set; }
    public List<long>? AssigneeIds { get; set; }
}
