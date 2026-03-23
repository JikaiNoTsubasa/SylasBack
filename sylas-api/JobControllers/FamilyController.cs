using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sylas_api.Database;
using sylas_api.Database.Models;
using sylas_api.Global;
using sylas_api.JobManagers;
using sylas_api.JobModels;
using sylas_api.JobModels.FamilyModel;

namespace sylas_api.JobControllers;

[AllowAnonymous]
public class FamilyController(SyContext context, FamilyManager familyManager) : SyController(context)
{
    private readonly FamilyManager _familyManager = familyManager;

    #region Members

    [HttpGet]
    [Route("api/family/members")]
    public IActionResult FetchMembers()
    {
        return Return(new ApiResult
        {
            Content = _familyManager.FetchMembers().Select(m => m.ToDTO()).ToList(),
            HttpCode = StatusCodes.Status200OK
        });
    }

    [HttpPost]
    [Route("api/family/member")]
    public IActionResult CreateMember([FromForm] RequestCreateFamilyMember model)
    {
        var member = _familyManager.CreateMember(model.Name, model.Avatar, model.Color, model.DisplayOrder);
        return Return(new ApiResult { Content = member.ToDTO(), HttpCode = StatusCodes.Status201Created });
    }

    [HttpPatch]
    [Route("api/family/member/{id}")]
    public IActionResult UpdateMember([FromRoute] long id, [FromForm] RequestUpdateFamilyMember model)
    {
        var member = _familyManager.UpdateMember(id, model.Name, model.Avatar, model.Color, model.DisplayOrder);
        return Return(new ApiResult { Content = member.ToDTO(), HttpCode = StatusCodes.Status200OK });
    }

    [HttpDelete]
    [Route("api/family/member/{id}")]
    public IActionResult DeleteMember([FromRoute] long id)
    {
        _familyManager.DeleteMember(id);
        return Return(new ApiResult { HttpCode = StatusCodes.Status200OK });
    }

    #endregion

    #region Tasks

    [HttpGet]
    [Route("api/family/tasks")]
    public IActionResult FetchTasks([FromQuery] long? memberId = null, [FromQuery] FamilyTaskTimeOfDay? timeOfDay = null, [FromQuery] DateOnly? date = null)
    {
        var today = date.HasValue ? date.Value.ToDateTime(TimeOnly.MinValue) : DateTime.UtcNow.Date;
        var tasks = _familyManager.FetchTasks(memberId, timeOfDay, today);
        return Return(new ApiResult
        {
            Content = tasks.Select(t => t.ToDTO(today)).ToList(),
            HttpCode = StatusCodes.Status200OK
        });
    }

    [HttpGet]
    [Route("api/family/task/{id}")]
    public IActionResult FetchTask([FromRoute] long id)
    {
        var task = _familyManager.FetchTask(id);
        return Return(new ApiResult { Content = task.ToDTO(DateTime.UtcNow.Date), HttpCode = StatusCodes.Status200OK });
    }

    [HttpPost]
    [Route("api/family/task")]
    public IActionResult CreateTask([FromForm] RequestCreateFamilyTask model)
    {
        var task = _familyManager.CreateTask(
            model.Name, model.Description, model.PointsReward, model.TimeOfDay,
            model.IsRecurring, model.RecurrenceDays, model.DisplayOrder, model.DueDate, model.AssigneeIds);
        return Return(new ApiResult { Content = task.ToDTO(DateTime.UtcNow.Date), HttpCode = StatusCodes.Status201Created });
    }

    [HttpPatch]
    [Route("api/family/task/{id}")]
    public IActionResult UpdateTask([FromRoute] long id, [FromForm] RequestUpdateFamilyTask model)
    {
        var task = _familyManager.UpdateTask(
            id, model.Name, model.Description, model.PointsReward, model.TimeOfDay,
            model.IsRecurring, model.RecurrenceDays, model.Status, model.DisplayOrder,
            model.DueDate, model.AssigneeIds);
        return Return(new ApiResult { Content = task.ToDTO(DateTime.UtcNow.Date), HttpCode = StatusCodes.Status200OK });
    }

    [HttpDelete]
    [Route("api/family/task/{id}")]
    public IActionResult DeleteTask([FromRoute] long id)
    {
        _familyManager.DeleteTask(id);
        return Return(new ApiResult { HttpCode = StatusCodes.Status200OK });
    }

    [HttpPost]
    [Route("api/family/task/{taskId}/complete/{memberId}")]
    public IActionResult CompleteTask([FromRoute] long taskId, [FromRoute] long memberId)
    {
        var task = _familyManager.CompleteTask(taskId, memberId);
        return Return(new ApiResult { Content = task.ToDTO(DateTime.UtcNow.Date), HttpCode = StatusCodes.Status200OK });
    }

    [HttpDelete]
    [Route("api/family/task/{taskId}/complete/{memberId}")]
    public IActionResult UncompleteTask([FromRoute] long taskId, [FromRoute] long memberId)
    {
        var task = _familyManager.UncompleteTask(taskId, memberId);
        return Return(new ApiResult { Content = task.ToDTO(DateTime.UtcNow.Date), HttpCode = StatusCodes.Status200OK });
    }

    #endregion
}
