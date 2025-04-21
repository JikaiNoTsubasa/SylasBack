namespace sylas_api.JobModels;

public record ResponseEntity
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
}
