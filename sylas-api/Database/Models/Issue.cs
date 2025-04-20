using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sylas_api.Database.Models;

public class Issue : Entity
{
    [ForeignKey(nameof(Project))]
    public long ProjectId { get; set; }
    public Project Project { get; set; } = null!;
    public IssueStatus Status { get; set; } = IssueStatus.NEW;
    public DevelopmentTime DevelopmentTime { get; set; } = DevelopmentTime.MEDIUM;

    [Range(1, 10)]
    public int Complexity { get; set; }
    public Priority Priority { get; set; } = Priority.LOW;
    public DateTime? DueDate { get; set; }
    [ForeignKey(nameof(Milestone))]
    public long MilestoneId { get; set; }
    public Milestone? Milestone { get; set; }
    public string? Description { get; set; }
    public string? GitlabTicket { get; set; }
    public List<Label>? Labels { get; set; }

    public List<IssueActivity>? Activities { get; set; }
    public List<Quest>? Quests { get; set; }
}
