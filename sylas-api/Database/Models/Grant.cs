using System;
using System.ComponentModel.DataAnnotations;

namespace sylas_api.Database.Models;

public class Grant
{
    [Key]
    public long Id { get; set; }
    public string Key { get; set; } = null!;

    public List<Role>? Roles { get; set; }
}
