using System;
using sylas_api.Database;
using sylas_api.Database.Models;
using sylas_api.Exceptions;
using sylas_api.Global;
using sylas_api.JobModels;
using sylas_api.JobModels.ProjectModel;

namespace sylas_api.JobManagers;

public class ProjectManager(SyContext context) : SyManager(context)
{
    public ApiResult FetchProjectsFiltered(QueryableEx.Pagination? pagination, QueryableEx.SearchQuery? search, QueryableEx.OrderQuery? order){
        List<ResponseProject> projects = [.. _context.Projects
            .Where(p => !p.IsDeleted)
            .Search(search, u => u.Name)
            .OrderBy(order, "name", u => u.Name)
            .Paged(pagination, out var meta)
            .Select(u => u.ToDTO())];
        return new ApiResult { Content = projects, Meta = meta, HttpCode = StatusCodes.Status200OK };
    }
    
    public Project CreateProject(string name, long? ownerId, long? customerId, long createdBy){
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
        project.MarkCreated(createdBy);
        return project;
    }

    public Project UpdateProject(long id, long createdBy, string? name = null, long? ownerId = null, long? customerId = null){
        Project project = _context.Projects.FirstOrDefault(p => p.Id == id) ?? throw new SyEntitiyNotFoundException($"Could not find project {id}");
        if (name != null) project.Name = name;
        if (ownerId != null) project.OwnerId = ownerId.Value;
        if (customerId != null) project.CustomerId = customerId.Value;
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
