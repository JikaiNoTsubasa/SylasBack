using System;
using System.ComponentModel.DataAnnotations;

namespace sylas_api.Database.Models;

public class ShoppingListItem
{
    [Key]
    public long Id { get; set; }
    public string? Name { get; set; }
    public int Quantity { get; set; }
}
