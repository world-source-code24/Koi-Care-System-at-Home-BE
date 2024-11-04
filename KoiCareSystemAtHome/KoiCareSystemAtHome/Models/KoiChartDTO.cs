using KoiCareSystemAtHome.Entities;

namespace KoiCareSystemAtHome.Models
{
    public class KoiChartDTO
    {
        public int ChartId { get; set; }

        public DateOnly Date { get; set; }

        public decimal Length { get; set; }

        public decimal Weight { get; set; }

        public string? HealthStatus { get; set; }

        public int KoiId { get; set; }
    }
}
