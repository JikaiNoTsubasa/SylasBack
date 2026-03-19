using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sylas_api.Database;
using sylas_api.JobManagers;
using sylas_api.JobModels;
using sylas_api.JobModels.TaskModel;

namespace sylas_api.JobControllers;

[AllowAnonymous]
public class TaskController(SyContext context, TaskManager manager) : SyController(context)
{
    protected TaskManager _manager = manager;

    #region Task List

    [HttpGet]
    [Route("api/task/list")]
    public IActionResult FetchAllTaskLists()
    {
        return Ok(_manager.FetchAllTaskLists().Select(s => s.ToDTO()));
    }

    [HttpGet]
    [Route("api/task/list/{id}")]
    public IActionResult FetchTaskList([FromRoute] long id)
    {
        return Ok(_manager.FetchTaskList(id).ToDTO());
    }

    [HttpPost]
    [Route("api/task/list")]
    public IActionResult CreateTaskList([FromBody] RequestCreateTaskList model)
    {
        return Ok(_manager.CreateTaskList(model.Name));
    }

    [HttpDelete]
    [Route("api/task/list/{id}")]
    public IActionResult DeleteTaskList([FromRoute] long id)
    {
        _manager.DeleteTaskList(id);
        return NoContent();
    }

    [HttpPut]
    [Route("api/task/list/{id}")]
    public IActionResult UpdateTaskList([FromRoute] long id, [FromBody] RequestUpdateTaskList model)
    {
        return Ok(_manager.UpdateTaskList(id, model.Name).ToDTO());
    }

    #endregion

    #region Item

    [HttpGet]
    [Route("api/task/list/{id}/item")]
    public IActionResult FetchAllTaskListItems([FromRoute] long id)
    {
        return Ok(_manager.FetchTaskListItems(id)?.Select(s => s.ToDTO()) ?? []);
    }

    [HttpPost]
    [Route("api/task/list/{id}/item")]
    public IActionResult CreateTaskListItem([FromRoute] long id, [FromBody] RequestCreateTaskListItem model)
    {
        return Ok(_manager.CreateTaskListItem(id, model.Name, model.Description));
    }

    [HttpPatch]
    [Route("api/task/list/{id}/item")]
    public IActionResult UpdateTaskListItem([FromRoute] long id, [FromBody] RequestUpdateTaskListItem model)
    {
        return Ok(_manager.UpdateTaskListItem(id, model.Name, model.Description, model.Status));
    }

    [HttpDelete]
    [Route("api/task/list/{id}/item")]
    public IActionResult DeleteTaskListItem([FromRoute] long id)
    {
        _manager.DeleteTaskListItem(id);
        return NoContent();
    }

    #endregion
}
