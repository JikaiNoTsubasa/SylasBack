namespace sylas_api.JobModels.UserModel;

public record ResponseUser : ResponseEntity
{
    public string Email { get; set; } = string.Empty;
    public string? Avatar { get; set; }

    public int XPFrontEnd { get; set; } = 0;
    public int XPBackEnd { get; set; } = 0;
    public int XPTests { get; set; } = 0;
    public int LevelFrontEnd { get; set; } = 0;
    public int LevelBackEnd { get; set; } = 0;
    public int LevelTests { get; set; } = 0;

    public string? Street { get; set; }
    public string? City { get; set; }
    public string? Zipcode { get; set; }
    public string? Country { get; set; }
}
