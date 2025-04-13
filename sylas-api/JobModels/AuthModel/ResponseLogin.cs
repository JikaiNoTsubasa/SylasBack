namespace sylas_api.JobModels.AuthModel;

public record ResponseLogin
{
    public string Token { get; set; } = null!;
}
