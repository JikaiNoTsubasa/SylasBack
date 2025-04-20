using System;

namespace sylas_api.Database.Models;

public class Document: Entity
{
    public string? Path { get; set; }

    public Project? Project { get; set; }
}
