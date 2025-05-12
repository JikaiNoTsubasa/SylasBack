using System.ComponentModel.DataAnnotations;
using sylas_api.Database.Models;

namespace sylas_api.JobModels.ProjectModel;

public record class RequestCreateQuest
{
    [Required]
    public long IssueId { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    public int? XPFrontEnd { get; set; }
    public int? XPBackEnd { get; set; }
    public int? XPTest { get; set; }
    public int? XPManagement { get; set; }

    public QuestStatus Status { get; set; } = QuestStatus.STARTED;
    public string? Description { get; set; }
    [Required]
    public long AssigneeId { get; set; }
}
