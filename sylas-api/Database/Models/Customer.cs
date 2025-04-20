using System;

namespace sylas_api.Database.Models;

public class Customer : Entity
{
    public List<User>? Members { get; set; }
    public List<Project>? Projects { get; set; }
}
