using System.ComponentModel.DataAnnotations;

namespace sylas_api.JobModels.PlanningModel;

public record class RequestUpdatePlanningItem
{
    public string? Name { get; set; }
    public DateTime PlannedDate { get; set; }
    public string? Description { get; set; }
    public long? UserId { get; set; }
}
