namespace sylas_api.JobModels.PreferenceModel;

public record ResponsePreference
{
    public int TimeHistory { get; set; }
    public int TimeChartMonth { get; set; }
}
