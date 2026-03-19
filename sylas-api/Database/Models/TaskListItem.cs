using System.ComponentModel.DataAnnotations;

namespace sylas_api.Database.Models;

public class TaskListItem
{
    [Key]
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public TaskListItemStatus Status { get; set; }
}
