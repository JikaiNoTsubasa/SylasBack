using System;
using Microsoft.EntityFrameworkCore;
using sylas_api.Database;
using sylas_api.Database.Models;
using sylas_api.Exceptions;

namespace sylas_api.JobManagers;

public class IssueManager(SyContext context) : SyManager(context)
{
    public Issue FetchIssue(long id){
        return _context.Issues
            .Include(i => i.Labels)
            .Include(i => i.Milestone)
            .Include(i => i.Quests)!.ThenInclude(q => q.Assignee)
            .Include(i => i.Activities)
            .FirstOrDefault(i => i.Id == id) 
            ?? throw new SyEntitiyNotFoundException($"Could not find issue {id}");
    }

    public List<Issue> FetchMyIssues(long userId, long? projectId = null){
        List<long> issueIds = [];

        // If project check if I am a member of that project or the owner of that project
        Project project = _context.Projects.FirstOrDefault(p => p.Id == projectId) ?? throw new SyEntitiyNotFoundException($"Could not find project {projectId}");

        // If owner of project, get all issues for that project
        if (project.OwnerId == userId) issueIds = [.. _context.Issues.Where(i => i.ProjectId == projectId && i.IsDeleted == false).Select(i => i.Id)];

        // If any quest assigned to me, get all issues for those quests
        List<long> questIds = [.. _context.Quests.Where(q => q.AssigneeId == userId && q.IsDeleted == false).Select(q => q.Id)]; 
        issueIds.AddRange([.. _context.Issues.Include(i => i.Quests).Where(i => i.Quests!.Any(q => questIds.Contains(q.Id))).Select(i => i.Id)]);

        issueIds = [.. issueIds.Distinct()];

        return [.. _context.Issues
            .Include(i => i.Labels)
            .Include(i => i.Milestone)
            .Include(i => i.Quests)!.ThenInclude(q => q.Assignee)
            .Include(i => i.Activities)
            .Where(i => issueIds.Contains(i.Id))];
    }

/*
    public List<Issue> FetchIssuesForProject(long projectId)
    {

    }
    */
}
