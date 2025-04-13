using System;

namespace sylas_api.Database.Models;

public class User : Entity
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public float XP { get; set; }
    public int Level { get; set; }

    public List<Role>? Roles { get; set; }
}
