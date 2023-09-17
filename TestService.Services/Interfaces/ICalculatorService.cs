using TestService.Services.Implementation;

namespace TestService.Services.Interfaces;

public interface ICalculatorService
{
    Task<IEnumerable<TariffInfo>> CalculateDeliveryCostAsync(string senderCityFias, string receiverCityFias, float weight,
        int length, int width, int height);
}