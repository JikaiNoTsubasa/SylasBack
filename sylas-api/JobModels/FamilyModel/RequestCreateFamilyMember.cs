namespace sylas_api.JobModels.FamilyModel;

public record RequestCreateFamilyMember
{
    public string Name { get; set; } = null!;
    public string? Avatar { get; set; }
    public string? Color { get; set; }
    public int DisplayOrder { get; set; } = 0;
}
