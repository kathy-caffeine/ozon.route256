using Route256.PriceCalculator.Domain.Bll.Models.PriceCalculator;

namespace Route256.PriceCalculator.Domain.Bll.Services.Interfaces;

public interface IPriceCalculatorService
{
    CalculationLogModel[] QueryLog(int take);
    decimal CalculatePrice(IReadOnlyList<GoodModel> goods);
    decimal CalculatePrice(CalculateRequest request);
}