namespace sylas_api.JobModels.GlobalParameterModel;

public record RequestUpdateGlobalParameter
{
    public string? Name { get; set; }
    public string? Value { get; set; }
    public string? Type { get; set; }
    public string? Description { get; set; }
}
