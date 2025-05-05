using sylas_api.Database.Models;

namespace sylas_api.JobModels.ProjectModel;

public record ResponseIssue : ResponseEntity
{
    public IssueStatus Status { get; set; }
    public DevelopmentTime DevelopmentTime { get; set; }
    public int Complexity { get; set; }
    public Priority Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public string? Description { get; set; }
    public string? GitlabTicket { get; set; }
    public ResponseMilestone? Milestone { get; set; }
    public List<ResponseLabel>? Labels { get; set; }
}
