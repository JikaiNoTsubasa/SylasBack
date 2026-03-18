using System;
using System.ComponentModel.DataAnnotations;

namespace sylas_api.Database.Models;

public class ShoppingList
{
    [Key]
    public long Id { get; set; }
    public string? Name { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<ShoppingListItem>? Items { get; set; }
}
