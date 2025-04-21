using System.ComponentModel.DataAnnotations;

namespace sylas_api.JobModels.ProjectModel;

public record RequestCreateProject
{
    [Required]
    public string Name { get; set; } = null!;
    public long? CustomerId { get; set; }
    public long? OwnerId { get; set; }
}
