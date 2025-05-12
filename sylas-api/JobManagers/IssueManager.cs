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
}
