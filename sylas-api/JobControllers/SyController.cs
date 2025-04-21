using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using sylas_api.Database;
using sylas_api.Global;

namespace sylas_api.JobControllers;

[ApiController]
public abstract class SyController(SyContext context) : Controller
{
    protected SyContext _context = context;
    protected long _loggedUserId = -1;

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var bearer = context.HttpContext.Request.Headers[HeaderNames.Authorization].FirstOrDefault()?.Split(" ").Last();
        if (bearer != null)
        {
            var userId = context.HttpContext.User.Claims.Where(c => c.Type.Equals("nameid")).Select(c => c.Value).FirstOrDefault();
            if (userId != null)
            {
                _loggedUserId = long.Parse(userId);
            }
        }

    }

    [NonAction]
    public virtual ObjectResult Return(ApiResult result){
        return StatusCode(result.HttpCode, result.Content);
    }
}
