using System;
using System.Collections.Generic;

namespace AnimeShop.Models;

public partial class Address
{
    public int AddressId { get; set; }

    public string? Street { get; set; }

    public int? ZipCode { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
