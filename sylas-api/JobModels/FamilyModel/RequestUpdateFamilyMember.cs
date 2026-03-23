namespace sylas_api.JobModels.FamilyModel;

public record RequestUpdateFamilyMember
{
    public string? Name { get; set; }
    public string? Avatar { get; set; }
    public string? Color { get; set; }
    public int? DisplayOrder { get; set; }
}
