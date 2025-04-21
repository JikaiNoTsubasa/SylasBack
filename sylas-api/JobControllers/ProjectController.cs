using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sylas_api.Database;
using sylas_api.Database.Models;
using sylas_api.Global;
using sylas_api.JobManagers;
using sylas_api.JobModels;
using sylas_api.JobModels.ProjectModel;

namespace sylas_api.JobControllers;

[Authorize]
public class ProjectController(SyContext context, ProjectManager projectManager) : SyController(context)
{
    protected readonly ProjectManager _projectManager = projectManager;

    [HttpGet]
    [Route("api/projects")]
    public IActionResult FetchProjects(){
        return Return(_projectManager.FetchProjectsFiltered(_pagination, _search, _orderby));
    }

    [HttpPost]
    [Route("api/project")]
    public IActionResult CreateProject([FromForm] RequestCreateProject model){
        ApiResult res = new(){
            HttpCode = StatusCodes.Status201Created,
            Content = _projectManager.CreateProject(model.Name, _loggedUserId, model.CustomerId, _loggedUserId).ToDTO(),
        };
        return Return(res);
    }

    [HttpPatch]
    [Route("api/project/{id}")]    
    public IActionResult UpdateProject([FromRoute] long id, [FromForm] RequestUpdateProject model){
        Project project = _projectManager.UpdateProject(id, _loggedUserId, model.Name, model.OwnerId, model.CustomerId);
        var res = new ApiResult(){ Content = project.ToDTO(), HttpCode = StatusCodes.Status200OK };
        return Return(res);
    }

    [HttpDelete]
    [Route("api/project/{id}")]    
    public IActionResult DeleteProject([FromRoute] long id){
        _projectManager.DeleteProject(id, _loggedUserId);
        var res = new ApiResult(){ HttpCode = StatusCodes.Status200OK };
        return Return(res);
    }
}
