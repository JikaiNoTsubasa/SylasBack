using System;
using sylas_api.Database.Models;

namespace sylas_api.JobModels.ShoppingModel;

public record ResponseShoppingList
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ItemsCount { get; set; }
    public ShoppingListStatus Status { get; set; }
}
