using sylas_api.Database.Models;
using sylas_api.JobModels.UserModel;

namespace sylas_api.JobModels.ProjectModel;

public record ResponseProject : ResponseEntity
{
    public ResponseUser? Owner { get; set; }
    public String? Description { get; set; }
    public List<ResponseIssue>? Issues { get; set; }
    public ProjectStatus Status { get; set; }
    public ResponseCustomer? Customer { get; set; }
}
