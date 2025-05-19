using sylas_api.Database.Models;

namespace sylas_api.JobModels.TodoModel;

public record class RequestUpdateTodo
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public TodoStatus? Status { get; set; }
    public DateTime? DueDate { get; set; }
}
