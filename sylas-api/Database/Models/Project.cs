using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace sylas_api.Database.Models;

public class Project : Entity
{
    [ForeignKey(nameof(Owner))]
    public long OwnerId { get; set; }
    public User Owner { get; set; } = null!;
    public List<Issue>? Issues { get; set; }
    public ProjectStatus Status { get; set; } = ProjectStatus.ACTIVE;
    [ForeignKey(nameof(Customer))]
    public long? CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public List<Document>? Documents { get; set; }
}
