namespace Route256.Week5.Homework.PriceCalculator.Api.Requests.V1;

/// <summary>
/// Вычисления, которые нужно удалить из истории. 
/// </summary>
public record ClearHistoryRequest(
    long UserId,
    long[] CalculationsIds
    );