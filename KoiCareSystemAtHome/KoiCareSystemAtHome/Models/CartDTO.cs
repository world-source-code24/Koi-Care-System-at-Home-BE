namespace KoiCareSystemAtHome.Models
{
    public class CartDTO
    {
        public int AccId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public override string? ToString()
        {
            return $"{AccId}, {Quantity}";
        }
    }
}
