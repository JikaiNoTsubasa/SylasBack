using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sylas_api.Database;
using sylas_api.Global;
using sylas_api.JobManagers;
using sylas_api.JobModels.TimeModel;

namespace sylas_api.JobControllers;

[Authorize]
public class TimeController(SyContext context, TimeManager timeManager) : SyController(context)
{
    protected TimeManager _timeManager = timeManager;
    
    [HttpPost]
    [Route("api/time")]
    public IActionResult AddTime([FromForm] RequestAddTime model){
        return Return(AddTimeToUser(model.UserId, model.Date, model.Minutes));
    }

    [HttpPost]
    [Route("api/time/me")]
    public IActionResult AddMyTime([FromForm] RequestAddTime model){
        return Return(AddTimeToUser(_loggedUserId, model.Date, model.Minutes));
    }

    [HttpGet]
    [Route("api/time/me/latest")]
    public IActionResult FetchMyLatestTimes(){
        return Return(new ApiResult(){ Content = _timeManager.FetchMyLatestTimes(_loggedUserId), HttpCode = StatusCodes.Status200OK }); 
    }

    [NonAction]
    private ApiResult AddTimeToUser(long userId, DateTime date, float minutes){ 
        _timeManager.AddTime(date, minutes, userId);
        return new(){ Content = null, HttpCode = StatusCodes.Status201Created };
    }
}
