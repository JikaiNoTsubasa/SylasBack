using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using sylas_api.Database;
using sylas_api.Exceptions;

namespace sylas_api.Global;

public class GrantHandler(SyContext context) : AuthorizationHandler<GrantRequirement>
{
    private readonly SyContext _context = context;

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GrantRequirement requirement)
    {
        var userId = context.User.FindFirst("userid")?.Value;

        if (userId != null){
            var hasGrant = _context.Users
                .Include(u => u.Roles)!.ThenInclude(r => r.Grants)
                .Where(u => u.Id == long.Parse(userId))
                .SelectMany(u => u.Roles!)
                .SelectMany(r => r.Grants!)
                .AnyAsync(g => g.Key.Equals(requirement.GrantName));
            if (hasGrant.Result){
                context.Succeed(requirement);
            }
        }
        return Task.CompletedTask;
    }
}
