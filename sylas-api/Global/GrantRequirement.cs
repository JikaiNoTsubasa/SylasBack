using System;
using Microsoft.AspNetCore.Authorization;

namespace sylas_api.Global;

public class GrantRequirement(string grantName) : IAuthorizationRequirement
{
    public string GrantName { get; } = grantName;
}
