namespace sylas_api.JobModels.PreferenceModel;

public record ResponsePreference:ResponseEntity
{
    public int TimeHistory { get; set; }
    public int TimeChartMonth { get; set; }
    public int TodoMaxDisplay { get; set; }
}
