using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sylas_api.Database;
using sylas_api.Global;
using sylas_api.JobManagers;
using sylas_api.JobModels;

namespace sylas_api.JobControllers;

[Authorize]
public class IssueController(SyContext context, IssueManager issueManager) : SyController(context)
{
    protected IssueManager _issueManager = issueManager;
    
    [HttpGet]
    [Route("api/issue/{id}")]    
    public IActionResult FetchIssue([FromRoute] long id){
        var res = new ApiResult(){ Content = _issueManager.FetchIssue(id).ToDTO(), HttpCode = StatusCodes.Status200OK };
        return Return(res);
    }
}
