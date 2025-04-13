using System;

namespace sylas_api.Database.Models;

public class Role : Entity
{
    public string Name { get; set; } = string.Empty;
    public string UniqueKey { get; set; } = string.Empty;
    public List<User>? Users { get; set; }
}
