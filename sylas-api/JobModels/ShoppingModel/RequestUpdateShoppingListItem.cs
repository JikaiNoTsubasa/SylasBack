using sylas_api.Database.Models;

namespace sylas_api.JobModels.ShoppingModel;

public record class RequestUpdateShoppingListItem
{
    public string? Name { get; set; }
    public int? Quantity { get; set; }
    public ShoppingListItemStatus? Status { get; set; }
}
