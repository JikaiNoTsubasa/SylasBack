using System;

namespace sylas_api.JobModels.ShoppingModel;

public class ResponseShoppingList
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ItemsCount { get; set; }
}
