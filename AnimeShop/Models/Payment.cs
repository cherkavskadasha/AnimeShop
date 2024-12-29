using System;
using System.Collections.Generic;

namespace AnimeShop.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public string? CardNumber { get; set; }

    public int? Cvv { get; set; }

    public string? Name { get; set; }

    public int? Month { get; set; }

    public int? Year { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
