using System;
using System.Collections.Generic;

namespace AnimeShop.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public int? Rating { get; set; }

    public string? ReviewText { get; set; }

    public DateOnly? ReviewDate { get; set; }

    public int? ProductId { get; set; }

    public int? CustomerId { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Product? Product { get; set; }
}
