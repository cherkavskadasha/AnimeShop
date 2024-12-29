using System;
using System.Collections.Generic;

namespace AnimeShop.Models;

public partial class Discount
{
    public int DiscountId { get; set; }

    public int? DiscountPercentage { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int? ProductId { get; set; }

    public virtual Product? Product { get; set; }
}
