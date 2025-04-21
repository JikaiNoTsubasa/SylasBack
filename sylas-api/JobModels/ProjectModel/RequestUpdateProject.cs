namespace sylas_api.JobModels.ProjectModel;

public record RequestUpdateProject
{
    public string? Name { get; set; }
    public long? CustomerId { get; set; }
    public long? OwnerId { get; set; }
}
