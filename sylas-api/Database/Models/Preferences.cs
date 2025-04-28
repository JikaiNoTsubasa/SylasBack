using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace sylas_api.Database.Models;

public class Preferences:Entity
{
    [ForeignKey(nameof(User))]
    public long UserId { get; set; }
    public User User { get; set; } = null!;
    public int TimeHistory { get; set; } = 10;
    public int TimeChartMonth { get; set; } = 12;
}
