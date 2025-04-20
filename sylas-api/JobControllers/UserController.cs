using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sylas_api.Database;
using sylas_api.Global;
using sylas_api.JobManagers;
using sylas_api.JobModels;

namespace sylas_api.JobControllers;

[Authorize]
public class UserController(SyContext context, UserManager userManager) : SyController(context)
{
    protected UserManager UserManager => userManager;

    [HttpGet]
    [Route("api/user/{id}")]
    public IActionResult GetUser([FromRoute] long id){
        var res = new ApiResult(){
            Content = UserManager.GetUser(id).ToDTO(),
            HttpCode = StatusCodes.Status200OK
        };

        return Return(res);
    }
}
