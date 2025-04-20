namespace sylas_api.JobModels;

public record ResponseEntity
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
}
