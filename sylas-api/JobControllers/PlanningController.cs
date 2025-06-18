using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sylas_api.Database;
using sylas_api.Global;
using sylas_api.JobManagers;
using sylas_api.JobModels;
using sylas_api.JobModels.PlanningModel;

namespace sylas_api.JobControllers;

[Authorize]
public class PlanningController(SyContext context, PlanningManager planningManager) : SyController(context)
{
    private readonly PlanningManager _planningManager = planningManager;

    [HttpGet]
    [Route("api/currentplannings")]
    public IActionResult FetchCurrentWeekPlanning()
    {
        var week = Engine.GetCurrentWeekNumber();
        var year = DateTime.Now.Year;
        (DateTime fist, DateTime last) = Engine.GetFirstAndLastDayOfWeek(year, week);
        var res = new ResponsePlanningWeek()
        {
            Week = week,
            Year = year,
            StartDate = fist,
            EndDate = last,
            PlanningItems = [.. _planningManager.FetchCurrentWeekPlanning().Select(p => p.ToDTO())]
        };
        return Return(new ApiResult() { Content = res, HttpCode = StatusCodes.Status200OK });
    }

    [HttpGet]
    [Route("api/planning/{week}/{year}")]
    public IActionResult FetchPlanningForWeek([FromRoute] int week, [FromRoute] int year)
    {
        (DateTime fist, DateTime last) = Engine.GetFirstAndLastDayOfWeek(year, week);
        var res = new ResponsePlanningWeek()
        {
            Week = week,
            Year = year,
            StartDate = fist,
            EndDate = last,
            PlanningItems = [.. _planningManager.FetchPlanningForWeek(week, year, _loggedUserId).Select(p => p.ToDTO())]
        };
        return Return(new ApiResult() { Content = res, HttpCode = StatusCodes.Status200OK });
    }

    [HttpPost]
    [Route("api/planning")]
    public IActionResult AddPlanningItem([FromForm] RequestCreatePlanningItem model)
    {
        var plan = _planningManager.AddPlanningItem(model.Name, model.PlannedDate, _loggedUserId, model.UserId, model.Description, model.IsPrivate);
        return Return(new ApiResult() { Content = plan.ToDTO(), HttpCode = StatusCodes.Status200OK });
    }

    [HttpPut]
    [Route("api/planning/{id}")]
    public IActionResult UpdatePlanningItem([FromRoute] long id, [FromForm] RequestUpdatePlanningItem model)
    {
        var plan = _planningManager.UpdatePlanningItem(id, _loggedUserId, model.Name, model.PlannedDate, model.UserId, model.Description, model.IsPrivate);
        return Return(new ApiResult() { Content = plan.ToDTO(), HttpCode = StatusCodes.Status200OK });
    }

    [HttpDelete]
    [Route("api/planning/{id}")]    
    public IActionResult DeletePlanningItem([FromRoute] long id) {
        _planningManager.DeletePlanningItem(id, _loggedUserId);
        return Return(new ApiResult() { Content = null, HttpCode = StatusCodes.Status204NoContent });    
    }
}
