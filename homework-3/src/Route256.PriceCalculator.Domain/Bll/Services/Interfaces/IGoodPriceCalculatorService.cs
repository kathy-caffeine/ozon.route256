namespace Route256.PriceCalculator.Domain.Bll.Services.Interfaces;

public interface IGoodPriceCalculatorService
{
    public decimal CalculatePrice(
        int id,
        decimal distance);
}