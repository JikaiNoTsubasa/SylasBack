namespace sylas_api.JobModels.TimeModel;

public record ResponseMyTimeInfo
{
    public int TotalTimeBalance { get; set; }
    public int MonthTimeBalance { get; set; }
}
