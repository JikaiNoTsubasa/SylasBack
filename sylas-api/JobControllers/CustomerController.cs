using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sylas_api.Database;
using sylas_api.Database.Models;
using sylas_api.Global;
using sylas_api.JobManagers;

namespace sylas_api.JobControllers;

[Authorize]
public class CustomerController(SyContext context, CustomerManager customerManager) : SyController(context)
{
    protected CustomerManager _customerManager = customerManager;

    [HttpGet]
    [Route("api/customers")]
    public IActionResult FetchCustomers()
    {
        return Return(new ApiResult(){ Content = _customerManager.FetchCustomers(), HttpCode = StatusCodes.Status200OK });
    }
}
