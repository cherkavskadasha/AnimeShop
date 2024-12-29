using System;
using System.Collections.Generic;

namespace AnimeShop.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public int? AddressId { get; set; }

    public int? PaymentId { get; set; }

    public virtual Address? Address { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Payment? Payment { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
