using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sylas_api.Database;
using sylas_api.JobManagers;
using sylas_api.JobModels;

namespace sylas_api.JobControllers;

[AllowAnonymous]
public class ShoppingController(SyContext context, ShoppingManager manager) : SyController(context)
{

    protected ShoppingManager _manager = manager;

    [HttpGet]
    [Route("api/shopping/list")]
    public IActionResult FetchAllShoppingLists()
    {
        return Ok(_manager.FetchAllShoppingLists().Select(s => s.ToDTO()));
    }
}
