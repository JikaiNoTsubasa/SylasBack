using System.ComponentModel.DataAnnotations;

namespace sylas_api.JobModels.DocumentModel;

public record RequestCreateDocument
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public IFormFile File { get; set; } = null!;
    public long? EntityId { get; set; }
}
