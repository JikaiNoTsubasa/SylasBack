using System;
using System.ComponentModel.DataAnnotations.Schema;
using sylas_api.Exceptions;

namespace sylas_api.Database.Models;

public class Document : Entity
{
    [ForeignKey(nameof(Entity))]
    public long? EntityId { get; set; }
    public Entity? Entity { get; set; }
    public List<DocumentVersion> Versions { get; set; } = [];

    [NotMapped]
    public DocumentVersion CurrentVersion { get
        {
            return Versions.Where(v => v.IsCurrent).FirstOrDefault() ?? throw new SyEntitiyNotFoundException($"Could not find current version for document {Id}");
        }
    }
}
