namespace sylas_api.JobModels.PlanningModel;

public record class ResponsePlanningWeek
{
    public int Week { get; set; }
    public int Year { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<ResponsePlanningItem>? PlanningItems { get; set; }
}
