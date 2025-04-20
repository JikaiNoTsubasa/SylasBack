using System;

namespace sylas_api.Database.Models;

public class Label : Entity
{
    public List<Issue>? Issues { get; set; }
}
