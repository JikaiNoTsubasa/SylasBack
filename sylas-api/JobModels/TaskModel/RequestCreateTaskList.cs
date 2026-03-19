using System.ComponentModel.DataAnnotations;

namespace sylas_api.JobModels.TaskModel;

public record class RequestCreateTaskList
{
    [Required]
    public string Name { get; set; } = null!;
}
