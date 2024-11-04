namespace KoiCareSystemAtHome.Models
{
    public class ProductDTO
    {
        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string? Image { get; set; }

        public string Category { get; set; } = null!;

        public string? Description { get; set; }

        public bool? Status { get; set; }

        public int? ShopId { get; set; }
    }
}
