namespace KoiCareSystemAtHome.Models
{
    public class WaterParameterDTO
    {
    
        public decimal? Temperature { get; set; }

        public decimal? Salt { get; set; }

        public decimal? PhLevel { get; set; }

        public decimal? O2Level { get; set; }

        public decimal? No2Level { get; set; }

        public decimal? No3Level { get; set; }

        public decimal? Po4Level { get; set; }

        public decimal? TotalChlorines { get; set; }


        public string? Note { get; set; }

        public List<string> CalculateMessage()
        {
            return new List<string> { $"Temperature: {TemperatureHealthy()}" ,
                    $"Salt: {SaltHealthy()}",
                    $"pH: {PhLevelHealthy()}",
                    $"Oxygen: {O2Healthy()}",
                    $"Nitrite: {No2Healthy()}",
                    $"Nitrate: {No3Healthy()}",
                    $"Phosphate: {Po4Healthy()}",
                    $"Chlorines: {TotalChlorinesHealthy()}"};
        }

        private string TemperatureHealthy()
        {
            string idealRange = " The ideal range is 18-24 Celcius";
            string message = "";
            if (Temperature > 24) message = "The temperature should be lower." + idealRange;
            else if (Temperature < 18) message = "The temperature should be higher." + idealRange;
            return message;
        }


        private string SaltHealthy()
        {
            string idealRange = " The ideal range is 0.1%-0.3%";
            string message = "";
            if (Salt < 0.0m) message = "The parameter should not be negative";
            else if (Salt > 0.3m) message = "The salt should be lower." + idealRange;
            else if (Salt < 0.1m) message = "The salt should be higher." + idealRange;
            return message;
        }

        private string PhLevelHealthy()
        {
            string idealRange = " The ideal range is 7.0-8.0";
            string message = "";
            if (PhLevel < 0.0m) message = "The parameter should not be negative";
            else if (PhLevel > 8.0m) message = "The pH should be lower." + idealRange;
            else if (PhLevel < 7.0m) message = "The ph should be higher." + idealRange;
            return message;
        }

        private string O2Healthy()
        {
            string idealRange = " The ideal range is 6.0-8.0";
            string message = "";
            if (O2Level < 0.0m) message = "The parameter should not be negative";
            else if (Salt > 8.0m) message = "The oxygen should be lower." + idealRange;
            else if (Salt < 6.0m) message = "The oxygen should be higher." + idealRange;
            return message;
        }

        private string No2Healthy()
        {
            string idealRange = " The nitrite should be as close to zero as possible";
            string message = "";
            if (No2Level < 0.0m) message = "The parameter should not be negative";
            else if (No2Level != 0.0m) message = idealRange;
            return message;
        }

        private string No3Healthy()
        {
            string idealRange = " The ideal range is 20-40 mg/L";
            string message = "";
            if (No3Level < 0.0m) message = "The parameter should not be negative";
            else if (No3Level > 40) message = "The nitrate should be lower." + idealRange;
            else if (No3Level < 20) message = "The nitrate should be higher." + idealRange;
            return message;
        }

        private string Po4Healthy()
        {
            string idealRange = " The ideal range is 0.5-2.0 mg/L";
            string message = "";
            if (No3Level < 0.0m) message = "The parameter should not be negative";
            else if (No3Level > 2.0m) message = "The phosphate should be lower." + idealRange;
            else if (No3Level < 0.5m) message = "The phosphate should be higher." + idealRange;
            return message;
        }

        private string TotalChlorinesHealthy()
        {
            string idealRange = "The target level should be 0.00mg/L";
            string message = "";
            if (No2Level < 0.0m) message = "The parameter should not be negative.";
            else if (No2Level != 0.0m) message = idealRange;
            return message;
        }
    }
}
