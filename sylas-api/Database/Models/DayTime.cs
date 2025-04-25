using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace sylas_api.Database.Models;

public class DayTime : Entity
{
    [ForeignKey(nameof(User))]
    public long UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTime Date { get; set; }
    public float Minutes { get; set; }
}
