using System.ComponentModel.DataAnnotations;

namespace sylas_api.JobModels.UserModel;

public record RequestCreateUser
{
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string Name { get; set; } = null!;
    public string? Avatar { get; set; }
    public string? Password { get; set; }

    public string? Street { get; set; }
    public string? City { get; set; }
    public string? Zipcode { get; set; }
    public string? Country { get; set; }
}
