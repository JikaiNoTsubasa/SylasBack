using System;
using log4net;
using Microsoft.EntityFrameworkCore;
using sylas_api.Database;
using sylas_api.Database.Models;
using sylas_api.Exceptions;
using sylas_api.Global;
using sylas_api.JobModels;
using sylas_api.JobModels.ProjectModel;

namespace sylas_api.JobManagers;

public class ProjectManager(SyContext context) : SyManager(context)
{

    protected ILog log = LogManager.GetLogger(typeof(ProjectManager));

    private IQueryable<Project> GetProjects(){ return _context.Projects.Include(p => p.Owner).Include(p => p.Customer); }

    public ApiResult FetchMyProjectsFiltered( long userId,QueryableEx.Pagination? pagination, QueryableEx.SearchQuery? search, QueryableEx.OrderQuery? order){
        log.Debug($"Fetching projects for user {userId}");
        // Get project ids where I am the owner
        List<long> projectIds = [..GetProjects().Where(p => p.OwnerId == userId).Select(p => p.Id)];
        log.Debug($"Found {projectIds.Count} projects for user {userId} as owner");

        // Get project ids where I am a member
        List<long> customerIds = [.._context.Customers.Include(c => c.Members).Where(c => c.Members!.Any(m => m.Id == userId)).Select(c => c.Id)];
        projectIds.AddRange([..GetProjects().Where(p => customerIds.Contains(p.CustomerId ?? 0)).Select(p => p.Id)]);
        log.Debug($"Found {customerIds.Count} projects for user {userId} as member");

        // Get project ids where I am the assignee
        List<long> questIds = [.._context.Quests.Include(q => q.Assignee).Where(q => q.AssigneeId == userId).Select(q => q.Id)];
        projectIds.AddRange([..GetProjects().Where(p => questIds.Contains(p.Id)).Select(p => p.Id)]);
        log.Debug($"Found {questIds.Count} projects for user {userId} as assignee");

        projectIds = [.. projectIds.Distinct()];
        log.Debug($"Found {projectIds.Count} distinct projects for user {userId}");

        List<ResponseProject> projects = [.. GetProjects()
            .Where(p => !p.IsDeleted)
            .Where(p => projectIds.Contains(p.Id))
            .Search(search, u => u.Name)
            .OrderBy(order, "name", u => u.Name)
            .Paged(pagination, out var meta)
            .Select(u => u.ToDTO())];

        return new ApiResult { Content = projects, Meta = meta, HttpCode = StatusCodes.Status200OK };
    }

    public ApiResult FetchProjectsFiltered(QueryableEx.Pagination? pagination, QueryableEx.SearchQuery? search, QueryableEx.OrderQuery? order){
        List<ResponseProject> projects = [.. GetProjects()
            .Where(p => !p.IsDeleted)
            .Search(search, u => u.Name)
            .OrderBy(order, "name", u => u.Name)
            .Paged(pagination, out var meta)
            .Select(u => u.ToDTO())];
        return new ApiResult { Content = projects, Meta = meta, HttpCode = StatusCodes.Status200OK };
    }

    public Project FetchProject(long id){ return GetProjects().FirstOrDefault(p => p.Id == id) ?? throw new SyEntitiyNotFoundException($"Could not find project {id}"); }
    
    public Project CreateProject(string name, long createdBy, string? description = null, long? ownerId = null, long? customerId = null){
        Project project = new(){
            Name = name,
        };
        // Setup owner
        if (ownerId != null){
            project.OwnerId = ownerId.Value;
        }else{
            project.OwnerId = createdBy;
        }
        if (customerId != null) project.CustomerId = customerId.Value;
        if (description != null) project.Description = description;
        project.MarkCreated(createdBy);
        _context.Projects.Add(project);
        _context.SaveChanges();
        return project;
    }

    public Project UpdateProject(long id, long createdBy, string? name = null, string? description = null, long? ownerId = null, long? customerId = null){
        Project project = _context.Projects.FirstOrDefault(p => p.Id == id) ?? throw new SyEntitiyNotFoundException($"Could not find project {id}");
        if (name != null) project.Name = name;
        if (ownerId != null) project.OwnerId = ownerId.Value;
        if (customerId != null) project.CustomerId = customerId.Value;
        if (description != null) project.Description = description;
        project.MarkUpdated(createdBy);
        _context.SaveChanges();
        return project;
    }

    public void DeleteProject(long id, long deletedBy){
        Project project = _context.Projects.FirstOrDefault(p => p.Id == id) ?? throw new SyEntitiyNotFoundException($"Could not find project {id}");
        project.MarkDeleted(deletedBy);
        _context.SaveChanges();
    }
}
