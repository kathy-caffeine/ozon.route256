namespace Route256.Week1.Homework.PriceCalculator.Api.Requests.V1;

/// <summary>
/// Товары. чью цену транспортировки нужно расчитать
/// </summary>
public record GoodCalculateRequest(
    int id,
    int distance
    );