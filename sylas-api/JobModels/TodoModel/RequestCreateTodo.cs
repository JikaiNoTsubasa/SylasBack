using System.ComponentModel.DataAnnotations;

namespace sylas_api.JobModels.TodoModel;

public record RequestCreateTodo
{
    [Required]
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
}
