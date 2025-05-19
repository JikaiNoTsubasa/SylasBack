using System;
using Microsoft.AspNetCore.Mvc;
using sylas_api.Database;
using sylas_api.Global;
using sylas_api.JobManagers;
using sylas_api.JobModels;
using sylas_api.JobModels.TodoModel;

namespace sylas_api.JobControllers;

public class TodoController(SyContext context, TodoManager todoManager) : SyController(context)
{
    protected TodoManager _todoManager = todoManager;

    [HttpGet]
    [Route("api/mytodos")]
    public IActionResult FetchMyTodos([FromQuery] bool? hasLimit = null)
    {
        return Return(new ApiResult() { Content = _todoManager.FetchMyTodos(_loggedUserId, hasLimit).Select(t => t.ToDTO()).ToList(), HttpCode = StatusCodes.Status200OK });
    }

    [HttpPost]
    [Route("api/todo")]
    public IActionResult CreateTodo([FromForm] RequestCreateTodo model)
    {
        return Return(new ApiResult() { Content = _todoManager.CreateTodo(_loggedUserId, model.Name, model.Description, model.DueDate).ToDTO(), HttpCode = StatusCodes.Status201Created });
    }

    [HttpPatch]
    [Route("api/todo/{id}")]
    public IActionResult UpdateTodo([FromRoute] long id, [FromForm] RequestUpdateTodo model)
    {
        return Return(new ApiResult() { Content = _todoManager.UpdateTodo(id, model.Name, model.Description, model.DueDate, model.Status).ToDTO(), HttpCode = StatusCodes.Status200OK });
    }
}
