using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sylas_api.Database.Models;

public class FamilyTaskCompletion
{
    [Key]
    public long Id { get; set; }

    [ForeignKey(nameof(Task))]
    public long TaskId { get; set; }
    public FamilyTask Task { get; set; } = null!;

    [ForeignKey(nameof(Member))]
    public long MemberId { get; set; }
    public FamilyMember Member { get; set; } = null!;

    public DateTime CompletedDate { get; set; }
    public int PointsEarned { get; set; }
}
