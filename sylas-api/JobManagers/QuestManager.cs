using System;
using sylas_api.Database;
using sylas_api.Database.Models;

namespace sylas_api.JobManagers;

public class QuestManager(SyContext context) : SyManager(context)
{
    public Quest CreateQuest(
        long createdBy,
        string name, 
        long issueId, 
        long assigneeId,
        string? description = null, 
        int? xpFront = null, 
        int? xpBack = null, 
        int? xpTest = null,
        int? xpManagement = null
    ){
        Quest quest = new(){
            Name = name,
            IssueId = issueId,
            AssigneeId = assigneeId
        };
        if (description != null) quest.Description = description;
        if (xpFront != null) quest.XPFrontEnd = xpFront.Value;
        if (xpBack != null) quest.XPBackEnd = xpBack.Value;
        if (xpTest != null) quest.XPTest = xpTest.Value;
        if (xpManagement != null) quest.XPManagement = xpManagement.Value;
        quest.MarkCreated(createdBy);
        _context.Quests.Add(quest);
        _context.SaveChanges();
        return quest;
    }
}
