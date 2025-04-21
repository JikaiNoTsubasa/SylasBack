namespace sylas_api.JobModels.UserModel;

public record RequestUpdateUser
{
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? Avatar { get; set; }
    public string? Password { get; set; }

    public string? Street { get; set; }
    public string? City { get; set; }
    public string? Zipcode { get; set; }
    public string? Country { get; set; }
}
