using System;
using Microsoft.AspNetCore.Mvc;
using sylas_api.Database;

namespace sylas_api.JobControllers;

[ApiController]
public class SyController(SyContext context) : ControllerBase
{
    protected SyContext _context = context;
}
