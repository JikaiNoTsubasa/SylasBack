using System;
using Microsoft.EntityFrameworkCore;
using sylas_api.Database;
using sylas_api.Database.Models;

namespace sylas_api.JobManagers;

public class TaskManager(SyContext context) : SyManager(context)
{
    public List<TaskList> FetchAllTaskLists()
    {
        return [.. _context.TaskLists.Include(s => s.Items!.Where(a => a.Status != TaskListItemStatus.DELETED)).Where(s => s.Status != TaskListStatus.DELETED).OrderByDescending(s => s.CreatedAt)];
    }

    public TaskList FetchTaskList(long id)
    {
        return _context.TaskLists.Find(id) ?? throw new Exception($"Could not find task list for id {id}");
    }

    public TaskList CreateTaskList(string name)
    {
        var taskList = new TaskList()
        {
            Name = name,
            CreatedAt = DateTime.UtcNow,
            Status = TaskListStatus.NEW
        };

        _context.TaskLists.Add(taskList);
        _context.SaveChanges();

        return taskList;
    }

    public void DeleteTaskList(long id)
    {
        var list = _context.TaskLists.Include(s => s.Items).FirstOrDefault(s => s.Id == id) ?? throw new Exception($"Could not find task list id {id}");
        list.Status = TaskListStatus.DELETED;
        if (list.Items != null && list.Items.Count > 0)
        {
            foreach (var it in list.Items)
            {
                it.Status = TaskListItemStatus.DELETED;
            }
        }
        _context.SaveChanges();
    }

    public TaskList UpdateTaskList(long id, string name)
    {
        var list = _context.TaskLists.Include(s => s.Items).FirstOrDefault(s => s.Id == id) ?? throw new Exception($"Could not find task list id {id}");
        list.Name = name;
        _context.SaveChanges();
        return list;
    }

    public List<TaskListItem>? FetchTaskListItems(long taskListId)
    {
        var list = _context.TaskLists.Include(s => s.Items!.Where(a => a.Status != TaskListItemStatus.DELETED).OrderByDescending(s => s.Id)).FirstOrDefault(s => s.Id == taskListId && s.Status != TaskListStatus.DELETED) ?? throw new Exception($"Could not find task list {taskListId}");
        return list.Items;
    }

    public TaskListItem CreateTaskListItem(long taskListId, string name, string? description)
    {
        var list = _context.TaskLists.Include(s => s.Items).FirstOrDefault(s => s.Id == taskListId) ?? throw new Exception($"Could not find task list {taskListId}");
        TaskListItem item = new()
        {
            Name = name,
            Description = description,
            Status = TaskListItemStatus.TODO
        };
        list.Items!.Add(item);
        _context.SaveChanges();
        return item;
    }

    public TaskListItem UpdateTaskListItem(long id, string? name = null, string? description = null, TaskListItemStatus? status = null)
    {
        var item = _context.TaskListItems.FirstOrDefault(s => s.Id == id) ?? throw new Exception($"Could not find task item {id}");
        if (name != null)
        {
            item.Name = name;
        }

        if (description != null)
        {
            item.Description = description;
        }

        if (status != null)
        {
            item.Status = status ?? TaskListItemStatus.TODO;
        }

        _context.SaveChanges();

        return item;
    }

    public void DeleteTaskListItem(long id)
    {
        var item = _context.TaskListItems.FirstOrDefault(s => s.Id == id) ?? throw new Exception($"Could not find task item {id}");
        item.Status = TaskListItemStatus.DELETED;
        _context.SaveChanges();
    }
}
