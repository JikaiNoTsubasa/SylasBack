using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace sylas_api.Database.Models;

public class Todo : Entity
{
    public string? Description { get; set; }

    public DateTime DueDate { get; set; }
    public TodoStatus Status { get; set; } = TodoStatus.TODO;

    [ForeignKey(nameof(Owner))]
    public long OwnerId { get; set; }
    public User? Owner { get; set; }
}
