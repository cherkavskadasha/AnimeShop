using System;
using System.Collections.Generic;

namespace AnimeShop.Models;

public partial class Order
{
    public int OrdersId { get; set; }

    public DateOnly? OrderDate { get; set; }

    public int? TotalAmount { get; set; }

    public string? Status { get; set; }

    public int? CustomerId { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
