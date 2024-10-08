namespace KoiCareSystemAtHome.Models
{
    public class OrderDetailsDTO
    {
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int? Quantity { get; set; }

        public decimal? TotalPrice { get; set; }
    }
}
