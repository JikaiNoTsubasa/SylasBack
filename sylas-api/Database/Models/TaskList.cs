using System;
using System.ComponentModel.DataAnnotations;

namespace sylas_api.Database.Models;

public class TaskList
{
    [Key]
    public long Id { get; set; }
    public string? Name { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<TaskListItem>? Items { get; set; }
    public TaskListStatus Status { get; set; }
}
