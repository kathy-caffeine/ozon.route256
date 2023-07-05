using System.Text.Json.Serialization;

namespace Route256.Week6.Homework.PriceCalculator.Bll.Kafka.Models;

public record RequestMessage(
    [property: JsonPropertyName("good_id")] long GoodId,
    [property: JsonPropertyName("height")] double Height,
    [property: JsonPropertyName("length")] double Length,
    [property: JsonPropertyName("width")] double Width,
    [property: JsonPropertyName("weight")] double Weight);
