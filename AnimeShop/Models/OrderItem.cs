using System;
using System.Collections.Generic;

namespace AnimeShop.Models;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int? Quantity { get; set; }

    public int? PricePerUnit { get; set; }

    public int? OrdersId { get; set; }

    public int? ProductId { get; set; }

    public virtual Order? Orders { get; set; }

    public virtual Product? Product { get; set; }
}
