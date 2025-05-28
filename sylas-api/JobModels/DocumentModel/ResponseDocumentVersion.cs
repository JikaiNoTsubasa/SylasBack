namespace sylas_api.JobModels.DocumentModel;

public record ResponseDocumentVersion
{
    public long Id { get; set; }
    public string Version { get; set; } = null!;
    public string Path { get; set; } = null!;
    public bool IsCurrent { get; set; }
}
