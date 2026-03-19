using System.ComponentModel.DataAnnotations;

namespace sylas_api.JobModels.ShoppingModel;

public record class RequestCreateShoppingListItem
{
    [Required]
    public string Name { get; set; } = null!;
    public int Quantity { get; set; } = 1;
}
