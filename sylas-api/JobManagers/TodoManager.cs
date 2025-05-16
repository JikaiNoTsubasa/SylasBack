using System;
using sylas_api.Database;
using sylas_api.Database.Models;
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
        if (description != null) todo.Drescription = description;
        if (dueDate != null) todo.DueDate = dueDate.Value;
        todo.MarkCreated(ownerId);
        _context.Todos.Add(todo);
        _context.SaveChanges();
        return todo;
    }
}
