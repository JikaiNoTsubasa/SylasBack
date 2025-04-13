using System;

namespace sylas_api.Database.Models;

public class User : Entity
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public float XP { get; set; } = 0;
    public int Level { get; set; } = 0;

    public List<Role>? Roles { get; set; }
}
