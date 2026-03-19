using System.ComponentModel.DataAnnotations;

namespace sylas_api.JobModels.TaskModel;

public record class RequestCreateTaskListItem
{
    [Required]
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
