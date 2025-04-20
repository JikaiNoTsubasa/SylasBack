using System;

namespace sylas_api.Database.Models;

public abstract class Entity
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }
    public long? CreatedBy { get; set; }

    public void MarkCreated(long userId){
        CreatedDate = DateTime.UtcNow;
        CreatedBy = userId;
    }
}
