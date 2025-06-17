using System.ComponentModel.DataAnnotations;

namespace sylas_api.JobModels.PlanningModel;

public record class RequestCreatePlanningItem
{
    [Required]
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public long? UserId { get; set; }
    [Required]
    public DateTime PlannedDate { get; set; }
}
