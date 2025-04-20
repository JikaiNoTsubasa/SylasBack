using System;

namespace sylas_api.Database.Models;

public class User : Entity
{
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public int XPFrontEnd { get; set; } = 0;
    public int XPBackEnd { get; set; } = 0;
    public int XPTests { get; set; } = 0;
    public int LevelFrontEnd { get; set; } = 0;
    public int LevelBackEnd { get; set; } = 0;
    public int LevelTests { get; set; } = 0;

    public List<Role>? Roles { get; set; }

    public List<Customer>? Customers { get; set; }
    public List<Project>? OwningProjects { get; set; }
    public List<Quest>? Quests { get; set; }
}
