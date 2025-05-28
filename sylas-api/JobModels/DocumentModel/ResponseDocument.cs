namespace sylas_api.JobModels.DocumentModel;

public record ResponseDocument : ResponseEntity
{
    public List<ResponseDocumentVersion> Versions { get; set; } = null!;
    public ResponseDocumentVersion CurrentVersion { get; set; } = null!;
}
