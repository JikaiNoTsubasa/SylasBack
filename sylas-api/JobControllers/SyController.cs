using System;
using System.ComponentModel.DataAnnotations.Schema;
using log4net;
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

    private static ILog log = LogManager.GetLogger(typeof(SyController));

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var user = context.HttpContext.User;
        if (user is not null){
            var ident = user.Identity;
            if (ident is not null && ident.IsAuthenticated){
                var nameid = user.Claims.FirstOrDefault(c => c.Type.Equals("userid"));
                if (nameid != null){
                    _loggedUserId = long.Parse(nameid.Value);
                }
            }
        }
        base.OnActionExecuting(context);
    }

    [NonAction]
    public virtual ObjectResult Return(ApiResult result){
        return StatusCode(result.HttpCode, result.Content);
    }
}
