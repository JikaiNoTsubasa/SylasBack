using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sylas_api.Database;

namespace sylas_api.JobControllers;

[Authorize]
public class PrivateController(SyContext context) : SyController(context)
{
    [HttpGet]
    [Route("api/auth/test")]
    public IActionResult GetTest(){
        return Ok("Test is ok");
    }
}
