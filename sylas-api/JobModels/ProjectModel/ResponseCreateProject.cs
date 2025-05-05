namespace sylas_api.JobModels.ProjectModel;

public record class ResponseCreateProject
{
    public long ProjectId { get; set; }
    public string ProjectName { get; set; } = null!;

    public int LevelBefore { get; set; }
    public int LevelAfter { get; set; }
    public int XpBefore { get; set; }
    public int XpAfter { get; set; }
    public int XpPercentBefore { get; set; }
    public int XpPercentAfter { get; set; }
    public int XpGained { get; set; }

}
