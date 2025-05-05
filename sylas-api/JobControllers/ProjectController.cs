using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sylas_api.Database;
using sylas_api.Database.Models;
using sylas_api.Exceptions;
using sylas_api.Global;
using sylas_api.JobManagers;
using sylas_api.JobModels;
using sylas_api.JobModels.ProjectModel;

namespace sylas_api.JobControllers;

[Authorize]
public class ProjectController(SyContext context, ProjectManager projectManager, GlobalParameterManager globalParameterManager) : SyController(context)
{
    protected readonly ProjectManager _projectManager = projectManager;
    protected readonly GlobalParameterManager _globalParameterManager = globalParameterManager;

    [HttpGet]
    [Route("api/projects")]
    public IActionResult FetchProjects(){
        return Return(_projectManager.FetchProjectsFiltered(_pagination, _search, _orderby));
    }

    [HttpGet]
    [Route("api/myprojects")]
    public IActionResult FetchMyProjects(){
        return Return(_projectManager.FetchMyProjectsFiltered(_loggedUserId,_pagination, _search, _orderby));
    }

    [HttpGet]
    [Route("api/project/{id}")]    
    public IActionResult FetchProject([FromRoute] long id){
        var res = new ApiResult(){ Content = _projectManager.FetchProject(id).ToDTO(), HttpCode = StatusCodes.Status200OK };
        return Return(res);
    }

    [HttpPost]
    [Route("api/project")]
    public IActionResult CreateProject([FromForm] RequestCreateProject model){
        long userId = _loggedUserId;
        Project prj =  _projectManager.CreateProject(
                name: model.Name,
                createdBy:_loggedUserId,
                customerId:model.CustomerId,
                description: model.Description,
                ownerId: model.OwnerId);

        // Get creator user
        User user = _context.Users.FirstOrDefault(u => u.Id == userId) ?? throw new SyEntitiyNotFoundException($"User {userId} not found");
        int XpPercentBefore = Engine.GetCurrentLevelXpPercent(user.LevelManagement, user.XPManagement);
        int XpBefore = user.XPManagement;
        int LevelBefore = user.LevelManagement;

        // Get parameter project xp
        int xp = _globalParameterManager.GetParameterValue(SyApplicationConstants.PARAM_PROJECT_CREATE_XP, 40);

        (int newLevel, int newXP) = Engine.AddXP(user.LevelManagement, user.XPManagement, xp);
        int XpPercentAfter = Engine.GetCurrentLevelXpPercent(newLevel, newXP);
        int XpAfter = newXP;
        int LevelAfter = newLevel;

        user.XPManagement = newXP;
        user.LevelManagement = newLevel;
        _context.SaveChanges();

        ResponseCreateProject resp = new(){
            ProjectId = prj.Id,
            ProjectName = prj.Name,
            XpPercentBefore = XpPercentBefore,
            XpBefore = XpBefore,
            LevelBefore = LevelBefore,
            XpPercentAfter = XpPercentAfter,
            XpAfter = XpAfter,
            LevelAfter = LevelAfter,
            XpGained = xp
        };

        ApiResult res = new(){
            HttpCode = StatusCodes.Status201Created,
            Content = resp
        };
        return Return(res);
    }

    [HttpPatch]
    [Route("api/project/{id}")]    
    public IActionResult UpdateProject([FromRoute] long id, [FromForm] RequestUpdateProject model){
        Project project = _projectManager.UpdateProject(id, _loggedUserId, model.Name, model.Description, model.OwnerId, model.CustomerId);
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
