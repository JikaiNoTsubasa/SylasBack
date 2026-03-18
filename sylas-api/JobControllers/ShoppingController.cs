using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sylas_api.Database;
using sylas_api.JobManagers;
using sylas_api.JobModels;
using sylas_api.JobModels.ShoppingModel;

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

    [HttpPost]
    [Route("api/shopping/list")]
    public IActionResult CreateShoppingList([FromBody] RequestCreateShoppingList model)
    {
        return Ok(_manager.CreateShoppingList(model.Name));
    }

    [HttpDelete]
    [Route("api/shopping/list/{id}")]
    public IActionResult CreateShoppingList([FromRoute] long id)
    {
        _manager.DeleteShoppingList(id);
        return NoContent();
    }
}
