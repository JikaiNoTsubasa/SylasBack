using sylas_api.JobModels.PreferenceModel;

namespace sylas_api.JobModels.UserModel;

public record ResponseUser : ResponseEntity
{
    public string Email { get; set; } = string.Empty;
    public string? Avatar { get; set; }

    public int XPFrontEnd { get; set; } = 0;
    public int XPBackEnd { get; set; } = 0;
    public int XPTests { get; set; } = 0;
    public int XPManagement { get; set; } = 0;
    public int LevelFrontEnd { get; set; } = 0;
    public int LevelBackEnd { get; set; } = 0;
    public int LevelTests { get; set; } = 0;
    public int LevelManagement { get; set; } = 0;
    public int XpPercentFrontEnd { get; set; }
    public int XpPercentBackEnd { get; set; }
    public int XpPercentTests { get; set; }
    public int XpPercentManagement { get; set; }

    public string? Street { get; set; }
    public string? City { get; set; }
    public string? Zipcode { get; set; }
    public string? Country { get; set; }

    public DateTime LastConnection { get; set; }
    public bool CanLogin { get; set; }

    public ResponsePreference? Preferences { get; set; }
}
