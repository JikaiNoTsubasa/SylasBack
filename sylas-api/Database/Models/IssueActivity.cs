using System;

namespace sylas_api.Database.Models;

public class IssueActivity : Entity
{
    public Issue Issue { get; set; } = null!;
}
