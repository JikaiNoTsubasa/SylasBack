using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sylas_api.Database;
using sylas_api.Global;
using sylas_api.JobManagers;
using sylas_api.JobModels;

namespace sylas_api.JobControllers;

[Authorize]
public class PreferenceController(SyContext context, PreferenceManager preferenceManager) : SyController(context)
{

    protected readonly PreferenceManager _preferenceManager = preferenceManager;

    [HttpGet]
    [Route("me/preferences")]
    public IActionResult FetchMyPreferences() {
        return Return(new ApiResult(){ Content = _preferenceManager.FetchMyPreferences(_loggedUserId).ToDTO(), HttpCode = StatusCodes.Status200OK }); 
    }
}
