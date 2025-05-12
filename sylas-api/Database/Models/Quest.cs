using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sylas_api.Database.Models;

public class Quest: Entity
{
    [ForeignKey(nameof(Assignee))]
    public long AssigneeId { get; set; }
    public User Assignee { get; set; } = null!;

    [ForeignKey(nameof(Issue))]
    public long IssueId { get; set; }
    public Issue Issue { get; set; } = null!;

    [Range(0, 100)]
    public int XPFrontEnd { get; set; }
    [Range(0, 100)]
    public int XPBackEnd { get; set; }
    [Range(0, 100)]
    public int XPTest { get; set; }
    [Range(0, 100)]
    public int XPManagement { get; set; }

    public QuestStatus Status { get; set; }
    public string? Description { get; set; }
}
