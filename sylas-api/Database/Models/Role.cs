using System;

namespace sylas_api.Database.Models;

public class Role : Entity
{
    public string UniqueKey { get; set; } = string.Empty;
    public List<User>? Users { get; set; }

    public List<Grant>? Grants { get; set; }
}
