namespace KoiCareSystemAtHome.Models
{
    public class KoiDTO
    {
        public int KoiId { get; set; }

        public string Name { get; set; } = null!;

        public string? Image { get; set; }

        public string? Physique { get; set; }

        public int Age { get; set; }

        public decimal Length { get; set; }

        public decimal Weight { get; set; }

        public bool Sex { get; set; }

        public string Breed { get; set; } = null!;

        public int? PondId { get; set; }
    }
}
