using sylas_api.JobModels.UserModel;

namespace sylas_api.JobModels.TimeModel;

public record ResponseTime : ResponseEntity
{
    public DateTime Date { get; set; }
    public ResponseUser User { get; set; } = null!;
    public float Minutes { get; set; }
}
