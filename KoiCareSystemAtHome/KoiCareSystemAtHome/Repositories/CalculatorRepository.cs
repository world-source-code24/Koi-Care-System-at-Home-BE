using KoiCareSystemAtHome.Repositories.IRepositories;

namespace KoiCareSystemAtHome.Repositories
{
    public class CalculatorRepository : ICalculatorRepository
    {
        public Task<float> foodCalculator(int numberKoi, float ratioFood, float weightKoi)
        {
            throw new NotImplementedException();
        }

        public Task<float> saltCalculator(float volume, float ratioSalt)
        {
            throw new NotImplementedException();
        }

        public Task<float> singleKoiFoodCalculator(float length, float weight, float ratioFood)
        {
            throw new NotImplementedException();
        }
    }
}
