namespace KoiCareSystemAtHome.Models
{
    public class PondDTO
    {
        public int PondId { get; set; }

        public string Name { get; set; } = null!;

        public string? Image { get; set; }

        public decimal? Depth { get; set; }

        public int? Volume { get; set; }

        public int? DrainCount { get; set; }

        public int? PumpCapacity { get; set; }

        public int UserId { get; set; }

        public PondDTO(int pondId, string name, string? image, decimal? depth, int? volume, int? drainCount, int? pumpCapacity, int userId)
        {
            PondId = pondId;
            Name = name;
            Image = image;
            Depth = depth;
            Volume = volume;
            DrainCount = drainCount;
            PumpCapacity = pumpCapacity;
            UserId = userId;
        }
    }

}
