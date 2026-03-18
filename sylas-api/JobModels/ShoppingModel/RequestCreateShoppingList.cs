using System.ComponentModel.DataAnnotations;

namespace sylas_api.JobModels.ShoppingModel;

public record class RequestCreateShoppingList
{
    [Required]
    public string Name { get; set; } = null!;
}
