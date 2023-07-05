using System.Text.Json.Serialization;

namespace Route256.Week1.Homework.PriceCalculator.Api.Responses.V3;

public record StatisticsResponse(
    [property: JsonPropertyName("max_weight")] int MaxWeight,
    [property: JsonPropertyName("max_volume")] int MaxVolume,
    [property: JsonPropertyName("max_distance_for_heaviest_good")] int MaxDistanceForHeaviestGood,
    [property: JsonPropertyName("max_distance_for_largest_good")] int MaxDistanceForLargestGood,
    [property: JsonPropertyName("wavg_price")] decimal WavgPrice
    );
