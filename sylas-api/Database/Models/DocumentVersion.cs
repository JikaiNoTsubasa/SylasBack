using System;
using System.ComponentModel.DataAnnotations;

namespace sylas_api.Database.Models;

public class DocumentVersion
{
    [Key]
    public long Id { get; set; }
    public Document Document { get; set; } = null!;
    public long DocumentId { get; set; }
    public string Version { get; set; } = null!;
    public string Path { get; set; } = null!;
    public bool IsCurrent { get; set; }
}
