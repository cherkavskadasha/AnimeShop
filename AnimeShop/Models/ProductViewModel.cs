using System.Collections.Generic;
using AnimeShop.Models;

namespace AnimeShop.Models;
public partial class ProductViewModel
    {
        public List<Product> Products { get; set; }
        public List<int?> WishlistItems { get; set; }
    }