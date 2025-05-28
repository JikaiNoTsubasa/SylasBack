using System;

namespace sylas_api.Database.Models;

public abstract class Entity
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }
    public long? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
    public long? UpdatedBy { get; set; }

    public DateTime? DeletedDate { get; set; }
    public long? DeletedBy { get; set; }

    public bool IsDeleted { get; set; }
    public List<Document>? Documents { get; set; }

    public void MarkCreated(long userId)
    {
        CreatedDate = DateTime.UtcNow;
        CreatedBy = userId;
    }

    public void MarkUpdated(long userId){
        UpdatedDate = DateTime.UtcNow;
        UpdatedBy = userId;
    }

    public void MarkDeleted(long userId){
        DeletedDate = DateTime.UtcNow;
        DeletedBy = userId;
        IsDeleted = true;
    }
}
