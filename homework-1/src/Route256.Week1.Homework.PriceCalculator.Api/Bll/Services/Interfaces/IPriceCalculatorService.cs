using Route256.Week1.Homework.PriceCalculator.Api.Bll.Models.PriceCalculator;

namespace Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.Interfaces;

public interface IPriceCalculatorService
{
    decimal CalculatePrice(IReadOnlyList<GoodModel> goods, int distance);

    CalculationLogModel[] QueryLog(int take);

    void ClearLog();

    void GetStatistics(out int max_weight,
        out int max_volume,
        out int max_distance_for_heaviest_good,
        out int max_distance_for_largest_good,
        out decimal wavg_price);
}