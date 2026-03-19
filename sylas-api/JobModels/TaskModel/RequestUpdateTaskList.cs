using System.ComponentModel.DataAnnotations;

namespace sylas_api.JobModels.TaskModel;

public record RequestUpdateTaskList
{
    [Required]
    public string Name { get; set; } = null!;
}
