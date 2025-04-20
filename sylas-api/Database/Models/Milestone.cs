using System;
using System.ComponentModel.DataAnnotations;

namespace sylas_api.Database.Models;

public class Milestone
{
    [Key]
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public List<Issue>? Issues { get; set; }
}
