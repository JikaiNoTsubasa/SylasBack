using System;
using sylas_api.Database.Models;

namespace sylas_api.JobModels.TaskModel;

public record ResponseTaskList
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ItemsCount { get; set; }
    public TaskListStatus Status { get; set; }
}
