using sylas_api.Database.Models;

namespace sylas_api.JobModels.TodoModel;

public record ResponseTodo : ResponseEntity
{
    public string? Description { get; set; }

    public DateTime DueDate { get; set; }
    public TodoStatus Status { get; set; } = TodoStatus.TODO;
    public long OwnerId { get; set; }
}
