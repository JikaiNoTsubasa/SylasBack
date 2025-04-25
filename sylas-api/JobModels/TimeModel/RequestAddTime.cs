namespace sylas_api.JobModels.TimeModel;

public record RequestAddTime
{
    public long UserId { get; set; }
    public DateTime Date { get; set; }
    public float Minutes { get; set; }
}
