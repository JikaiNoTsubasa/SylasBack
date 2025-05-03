using sylas_api.JobModels.UserModel;

namespace sylas_api.JobModels.ProjectModel;

public record ResponseProject : ResponseEntity
{
    public ResponseUser? Owner { get; set; }
    public String? Description { get; set; }
}
