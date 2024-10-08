namespace KoiCareSystemAtHome.Models
{
    public class OrderDTO
    {
        public int OrderId { get; set; }

        public DateOnly Date { get; set; }

        public string? StatusOrder { get; set; }

        public string? StatusPayment { get; set; }

        public decimal? TotalAmount { get; set; }

        public int? AccId { get; set; }
    }
}
