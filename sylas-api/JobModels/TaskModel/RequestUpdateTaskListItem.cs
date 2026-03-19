using sylas_api.Database.Models;

namespace sylas_api.JobModels.TaskModel;

public record class RequestUpdateTaskListItem
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public TaskListItemStatus? Status { get; set; }
}
