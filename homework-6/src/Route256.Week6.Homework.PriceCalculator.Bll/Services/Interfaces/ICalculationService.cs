using Route256.Week5.Workshop.PriceCalculator.Bll.Models;

namespace Route256.Week5.Workshop.PriceCalculator.Bll.Services.Interfaces;

public interface ICalculationService
{
    Task<long> SaveCalculation(
        SaveCalculationModel saveCalculation,
        CancellationToken cancellationToken);

    decimal CalculatePriceByVolume(GoodModel good);

    public decimal CalculatePriceByWeight(GoodModel good);

    decimal CalculatePriceByVolume(GoodModel[] goods, out double volume);

    public decimal CalculatePriceByWeight(GoodModel[] goods, out double weight);

    Task<QueryCalculationModel[]> QueryCalculations(
        QueryCalculationFilter query,
        CancellationToken token);

    public decimal CalculatePrice(GoodModel good);
}