using System.Text.Json.Serialization;

namespace Route256.Week6.Homework.PriceCalculator.Bll.Kafka.Models;

public record OrderMessage(
    [property: JsonPropertyName("good_id")] long GoodId,
    [property: JsonPropertyName("price")] decimal Price
    );
