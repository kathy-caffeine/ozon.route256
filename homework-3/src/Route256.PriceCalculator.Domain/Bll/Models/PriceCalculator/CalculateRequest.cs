namespace Route256.PriceCalculator.Domain.Bll.Models.PriceCalculator;

public sealed record CalculateRequest(GoodModel[] Goods, decimal Distance);