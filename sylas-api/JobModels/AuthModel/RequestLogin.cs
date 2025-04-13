using System;

namespace sylas_api.JobModels.AuthModel;

public record RequestLogin
{
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
}
