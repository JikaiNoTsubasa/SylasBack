using System;

namespace sylas_api.Database.Models;

public class GlobalParameter
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string Value { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string? Description { get; set; }
}
