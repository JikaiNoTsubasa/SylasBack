using sylas_api.JobModels.UserModel;

namespace sylas_api.JobModels.PlanningModel;

public record class ResponsePlanningItem : ResponseEntity
{
    public string? Description { get; set; }
    public DateTime PlannedDate { get; set; }
    public ResponseUser? User { get; set; }
    public ResponseUser? Owner { get; set; }

}
