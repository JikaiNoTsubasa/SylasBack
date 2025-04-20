using System;
using Microsoft.AspNetCore.Mvc;
using sylas_api.Database;
using sylas_api.Global;

namespace sylas_api.JobControllers;

[ApiController]
public class SyController(SyContext context) : ControllerBase
{
    protected SyContext _context = context;

    public virtual ObjectResult Return(ApiResult result){
        return StatusCode(result.HttpCode, result.Content);
    }
}
