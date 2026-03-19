using System;
using Microsoft.EntityFrameworkCore;
using sylas_api.Database;
using sylas_api.Database.Models;

namespace sylas_api.JobManagers;

public class ShoppingManager(SyContext context) : SyManager(context)
{
    public List<ShoppingList> FetchAllShoppingLists()
    {
        return [.. _context.ShoppingLists.Include(s => s.Items).Where(s => s.Status != ShoppingListStatus.DELETED).OrderByDescending(s => s.CreatedAt)];
    }

    public ShoppingList FetchShoppingList(long id)
    {
        return _context.ShoppingLists.Find(id) ?? throw new Exception($"Could not find shopping info for id {id}");
    }

    public ShoppingList CreateShoppingList(string name)
    {
        var shopList = new ShoppingList()
        {
            Name = name,
            CreatedAt = DateTime.UtcNow,
            Status = ShoppingListStatus.NEW
        };

        _context.ShoppingLists.Add(shopList);
        _context.SaveChanges();

        return shopList;
    }

    public void DeleteShoppingList(long id)
    {
        var list = _context.ShoppingLists.FirstOrDefault(s => s.Id == id) ?? throw new Exception($"Could not find shopping list id {id}");
        list.Status = ShoppingListStatus.DELETED;
        if (list.Items != null && list.Items.Count > 0)
        {
            foreach(var it in list.Items)
            {
                it.Status = ShoppingListItemStatus.DELETED;
            }
        }
    }

    public ShoppingList UpdateShoppingList(long id, string name)
    {
        var list = _context.ShoppingLists.Include(s => s.Items).FirstOrDefault(s => s.Id == id) ?? throw new Exception($"Could not find shopping list id {id}");
        list.Name = name;
        _context.SaveChanges();
        return list;
    }

    public List<ShoppingListItem>? FecthShoppingListItems(long shoppingListId)
    {
        var list = _context.ShoppingLists.Include(s => s.Items!.Where(a => a.Status != ShoppingListItemStatus.DELETED)).FirstOrDefault(s => s.Id == shoppingListId && s.Status != ShoppingListStatus.DELETED) ?? throw new Exception($"Could not find shopping list {shoppingListId}");

        return list.Items;
    }

    public ShoppingListItem CreateShoppingListItem(long shoppingListId, string name, int quantity)
    {
        var list = _context.ShoppingLists.Include(s => s.Items).FirstOrDefault(s => s.Id == shoppingListId) ?? throw new Exception($"Could not find shopping list {shoppingListId}");
        ShoppingListItem item = new()
        {
            Name = name,
            Quantity = quantity
        };
        list.Items!.Add(item);
        _context.SaveChanges();
        return item;
    }
}
