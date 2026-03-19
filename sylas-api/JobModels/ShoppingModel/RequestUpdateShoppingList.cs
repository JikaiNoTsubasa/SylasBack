using System.ComponentModel.DataAnnotations;

namespace sylas_api.JobModels.ShoppingModel;

public record RequestUpdateShoppingList
{
    [Required]
    public string Name { get; set; } = null!;
}
