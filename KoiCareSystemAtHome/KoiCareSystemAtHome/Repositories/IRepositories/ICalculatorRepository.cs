namespace KoiCareSystemAtHome.Repositories.IRepositories
{
    public interface ICalculatorRepository
    {
        Task<float> foodCalculator(int numberKoi, float ratioFood, float weightKoi);
        Task<float> saltCalculator(float volume, float ratioSalt);
        Task<float> singleKoiFoodCalculator(float length, float weight, float ratioFood);
    }
}
