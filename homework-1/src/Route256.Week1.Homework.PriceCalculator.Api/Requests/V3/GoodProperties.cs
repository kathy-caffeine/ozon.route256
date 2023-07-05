namespace Route256.Week1.Homework.PriceCalculator.Api.Requests.V3;

/// <summary>
/// Харектеристики товара
/// </summary>
public record GoodProperties(
    int Height,
    int Length,
    int Width,
    int Weight);