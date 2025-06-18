using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace sylas_api.Database.Models;

public class PlanningItem : Entity
{
    public string? Description { get; set; }

    [ForeignKey(nameof(User))]
    public long? UserId { get; set; }
    public User? User { get; set; }

    public DateTime PlannedDate { get; set; }
    
    [ForeignKey(nameof(Owner))]
    public long? OwnerId { get; set; }
    public User? Owner { get; set; }
}
