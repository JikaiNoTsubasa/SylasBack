namespace sylas_api.JobModels.GlobalParameterModel;

public record ResponseGlobalParameter
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string Value { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string? Description { get; set; }
}
