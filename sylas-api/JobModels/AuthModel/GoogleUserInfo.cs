using System.ComponentModel.DataAnnotations;

namespace sylas_api.JobModels.AuthModel;

public record GoogleUserInfo
{
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string Name { get; set; } = null!;
}
