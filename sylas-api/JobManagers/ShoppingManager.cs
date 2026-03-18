using System;
using Microsoft.EntityFrameworkCore;
using sylas_api.Database;
using sylas_api.Database.Models;

namespace sylas_api.JobManagers;

public class ShoppingManager(SyContext context) : SyManager(context)
{
    public List<ShoppingList> FetchAllShoppingLists()
    {
        return [.. _context.ShoppingLists.Include(s => s.Items).OrderByDescending(s => s.CreatedAt)];
    }
}
