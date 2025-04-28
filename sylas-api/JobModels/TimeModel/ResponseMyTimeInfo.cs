namespace sylas_api.JobModels.TimeModel;

public record ResponseMyTimeInfo
{
    public int TotalTimeBalance { get; set; }
    public int TotalEntries { get; set; }
    public int MonthTimeBalance { get; set; }
    public float MoyBalance { get; set; }
    public List<ResponseTotalByMonth>? TotalByMonth { get; set; }
}
