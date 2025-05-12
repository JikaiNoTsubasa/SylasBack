using System.ComponentModel.DataAnnotations;
using sylas_api.Database.Models;

namespace sylas_api.JobModels.ProjectModel;

public record RequestCreateIssue
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public long ProjectId { get; set; }
    [Required]
    public DevelopmentTime DevelopmentTime { get; set; }
    [Required]
    public int Complexity { get; set; }
    [Required]
    public Priority Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public string? Description { get; set; }
    public long? MilestoneId { get; set; }
    public string? GitlabTicket { get; set; }
    public List<long>? LabelIds { get; set; }
}
