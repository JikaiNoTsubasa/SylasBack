using System;
using Microsoft.AspNetCore.Mvc;
using sylas_api.Database;
using sylas_api.Database.Models;
using sylas_api.Global;
using sylas_api.JobManagers;
using sylas_api.JobModels;
using sylas_api.JobModels.ProjectModel;

namespace sylas_api.JobControllers;

public class QuestController(SyContext context, QuestManager questManager) : SyController(context)
{
    protected QuestManager _questManager = questManager;

    [HttpPost]
    [Route("api/quest")]
    public IActionResult CreateQuest([FromForm] RequestCreateQuest model)
    {
        Quest quest = _questManager.CreateQuest(_loggedUserId, model.Name, model.IssueId, model.AssigneeId, model.Description, model.XPFrontEnd, model.XPBackEnd, model.XPTest, model.XPManagement);
        return Return(new ApiResult() { Content = quest.ToDTO(), HttpCode = StatusCodes.Status201Created });
    }
}
