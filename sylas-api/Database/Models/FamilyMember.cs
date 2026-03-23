using System.ComponentModel.DataAnnotations;

namespace sylas_api.Database.Models;

public class FamilyMember
{
    [Key]
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Avatar { get; set; }
    public string? Color { get; set; }
    public int TotalPoints { get; set; } = 0;
    public int DisplayOrder { get; set; } = 0;
    public DateTime CreatedAt { get; set; }

    public List<FamilyTask>? Tasks { get; set; }
    public List<FamilyTaskCompletion>? Completions { get; set; }
}
