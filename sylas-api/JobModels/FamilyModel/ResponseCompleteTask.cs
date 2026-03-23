namespace sylas_api.JobModels.FamilyModel;

public record ResponseCompleteTask
{
    public ResponseFamilyTask Task { get; set; } = null!;
    public ResponseFamilyMember Member { get; set; } = null!;
}
