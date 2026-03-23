namespace sylas_api.JobModels.FamilyModel;

public record ResponseFamilyMember
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Avatar { get; set; }
    public string? Color { get; set; }
    public int TotalPoints { get; set; }
    public int DisplayOrder { get; set; }
    public DateTime CreatedAt { get; set; }
}
