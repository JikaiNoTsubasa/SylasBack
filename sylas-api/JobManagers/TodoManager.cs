using System;
using sylas_api.Database;
using sylas_api.Database.Models;
using sylas_api.Exceptions;
using sylas_api.Global;

namespace sylas_api.JobManagers;

public class TodoManager(SyContext context, PreferenceManager preferenceManager) : SyManager(context)
{
    protected PreferenceManager _preferenceManager = preferenceManager;

    public List<Todo> FetchMyTodos(long userId, bool? hasLimit = null)
    {
        int? limit = hasLimit is null ? _preferenceManager.FetchMyPreferences(userId).TodoMaxDisplay : null;
        return [.. _context.Todos
            .Where(t => t.OwnerId == userId && t.IsDeleted == false && t.Status != TodoStatus.DONE)
            .Limit(limit)
            .OrderByDescending(t => t.CreatedDate)];
    }

    public Todo CreateTodo(long ownerId, string name, string? description = null, DateTime? dueDate = null)
    {
        Todo todo = new()
        {
            Name = name,
            OwnerId = ownerId,
        };
        if (description != null) todo.Description = description;
        if (dueDate != null) todo.DueDate = dueDate.Value;
        todo.MarkCreated(ownerId);
        _context.Todos.Add(todo);
        _context.SaveChanges();
        return todo;
    }

    public Todo UpdateTodo(long id, string? name = null, string? description = null, DateTime? dueDate = null, TodoStatus? status = null)
    {
        Todo todo = _context.Todos.FirstOrDefault(t => t.Id == id) ?? throw new SyEntitiyNotFoundException($"Could not find todo {id}");
        if (name != null) todo.Name = name;
        if (description != null) todo.Description = description;
        if (dueDate != null) todo.DueDate = dueDate.Value;
        if (status != null) todo.Status = status.Value;
        todo.MarkUpdated(id);
        _context.SaveChanges();
        return todo;
    }
}
