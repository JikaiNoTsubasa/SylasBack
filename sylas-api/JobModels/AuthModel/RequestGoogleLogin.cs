using System.ComponentModel.DataAnnotations;

namespace sylas_api.JobModels.AuthModel;

public record RequestGoogleLogin
{
    [Required]
    public string AccessToken { get; set; } = null!;
}
