namespace Route256.PriceCalculator.Domain.Bll;

public sealed class PriceCalculatorOptions
{
    public decimal VolumeToPriceRatio { get; set; }
    public decimal WeightToPriceRatio { get; set; }
}