using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace sylas_api.Database.Models;

public class User : Entity
{
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public bool CanLogin { get; set; } = true;

    public int XPFrontEnd { get; set; } = 0;
    public int XPBackEnd { get; set; } = 0;
    public int XPTests { get; set; } = 0;
    public int XPManagement { get; set; } = 0;

    public int LevelFrontEnd { get; set; } = 0;
    public int LevelBackEnd { get; set; } = 0;
    public int LevelTests { get; set; } = 0;
    public int LevelManagement { get; set; } = 0;

    public DateTime LastConnection { get; set; }

    public string? Street { get; set; }
    public string? City { get; set; }
    public string? Zipcode { get; set; }
    public string? Country { get; set; }

    public List<Role>? Roles { get; set; }

    public List<Customer>? Customers { get; set; }
    public List<Project>? OwningProjects { get; set; }
    public List<Quest>? Quests { get; set; }
    public List<DayTime>? Times { get; set; }
    public List<Todo>? Todos { get; set; }

    public Preferences? Preferences { get; set; }

    public List<PlanningItem>? PlanningItems { get; set; }
}
