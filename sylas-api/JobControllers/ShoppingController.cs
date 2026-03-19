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

    #region Shopping List

    [HttpGet]
    [Route("api/shopping/list")]
    public IActionResult FetchAllShoppingLists()
    {
        return Ok(_manager.FetchAllShoppingLists().Select(s => s.ToDTO()));
    }

    [HttpGet]
    [Route("api/shopping/list/{id}")]
    public IActionResult FetchAllShoppingLists([FromRoute] long id)
    {
        return Ok(_manager.FetchShoppingList(id).ToDTO());
    }

    [HttpPost]
    [Route("api/shopping/list")]
    public IActionResult CreateShoppingList([FromBody] RequestCreateShoppingList model)
    {
        return Ok(_manager.CreateShoppingList(model.Name));
    }

    [HttpDelete]
    [Route("api/shopping/list/{id}")]
    public IActionResult DeleteShoppingList([FromRoute] long id)
    {
        _manager.DeleteShoppingList(id);
        return NoContent();
    }

    [HttpPut]
    [Route("api/shopping/list/{id}")]
    public IActionResult UpdateShoppingList([FromRoute] long id, [FromBody] RequestUpdateShoppingList model)
    {
        return Ok(_manager.UpdateShoppingList(id, model.Name).ToDTO());
    }

    #endregion

    #region Item
    [HttpGet]
    [Route("api/shopping/list/{id}/item")]
    public IActionResult FetchAllShoppingListItems([FromRoute] long id)
    {
        return Ok(_manager.FecthShoppingListItems(id)?.Select(s => s.ToDTO()) ?? []);
    }

    [HttpPost]
    [Route("api/shopping/list/{id}/item")]
    public IActionResult CreateShoppingListItem([FromRoute] long id, [FromBody] RequestCreateShoppingListItem model)
    {
        return Ok(_manager.CreateShoppingListItem(id, model.Name, model.Quantity));
    }

    [HttpPatch]
    [Route("api/shopping/list/{id}/item")]
    public IActionResult UpdateShoppingListItem([FromRoute] long id, [FromBody] RequestUpdateShoppingListItem model)
    {
        return Ok(_manager.UpdateShoppingListItem(id, model.Name, model.Quantity, model.Status));
    }

    [HttpDelete]
    [Route("api/shopping/list/{id}/item")]
    public IActionResult DeleteShoppingListItem([FromRoute] long id)
    {
        _manager.DeleteShoppingListItem(id);
        return NoContent();
    }
    #endregion
}
