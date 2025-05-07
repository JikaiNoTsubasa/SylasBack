using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sylas_api.Database;
using sylas_api.Global;
using sylas_api.JobManagers;
using sylas_api.JobModels;
using sylas_api.JobModels.GlobalParameterModel;

namespace sylas_api.JobControllers;

[Authorize]
public class GlobalParameterController(SyContext context, GlobalParameterManager gpManager) : SyController(context)
{

    protected readonly GlobalParameterManager _gpManager = gpManager;

    [HttpGet]
    [Route("api/globalparameters")]
    [Authorize(Policy = "GP.ListAll")]
    public IActionResult FetchAllParameters()
    {
        return Return(new ApiResult(){ Content = _gpManager.FetchAllParameters().Select(p => p.ToDTO()).ToList(), HttpCode = StatusCodes.Status200OK });
    }

    [HttpPatch]
    [Route("api/globalparameter/{id}")]
    [Authorize(Policy = "GP.Update")]
    public IActionResult UpdateParameter([FromRoute] long id, [FromForm] RequestUpdateGlobalParameter model){
        return Return(new ApiResult(){ Content = _gpManager.UpdateParameter(id, model.Name, model.Value, model.Type, model.Description).ToDTO(), HttpCode = StatusCodes.Status200OK });
    }
}
