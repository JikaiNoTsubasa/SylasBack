namespace sylas_api.JobModels.TimeModel;

public record ResponseTotalByMonth
{
    public int Month { get; set; }
    public int Year { get; set; }
    public int Total { get; set; }
}
