using System;
using System.Collections.Generic;
using System.Text;

namespace AnimeShop.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? Price { get; set; }

    public int? Stock { get; set; }

    public string? Image { get; set; }

    public int? CategoryId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Discount> Discounts { get; set; } = new List<Discount>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();

    public string GetStarRating(object averageRatingObj)
    {
        double averageRating = Convert.ToDouble(averageRatingObj);
        int fullStars = (int)Math.Floor(averageRating);
        bool hasHalfStar = averageRating % 1 >= 0.5;

        var starRating = new StringBuilder();
        for (int i = 0; i < fullStars; i++)
        {
            starRating.Append("⭐");
        }
        if (hasHalfStar)
        {
            starRating.Append("✩");
        }

        return starRating.ToString();
    }
}
