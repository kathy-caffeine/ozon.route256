using Confluent.Kafka;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Route256.Week6.Homework.PriceCalculator.Bll.Kafka;

public sealed class JsonValueSerializer<T> : ISerializer<T>, IDeserializer<T>
{
    private static readonly JsonSerializerOptions _serializerOptions;

    static JsonValueSerializer()
    {
        _serializerOptions = new JsonSerializerOptions();
        _serializerOptions.Converters.Add(new JsonStringEnumConverter());
    }

    public byte[] Serialize(T data, SerializationContext context) => JsonSerializer.SerializeToUtf8Bytes(data, _serializerOptions);

    public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull)
            throw new ArgumentNullException(nameof(data), "Null data encountered");

        return JsonSerializer.Deserialize<T>(data, _serializerOptions) ??
               throw new ArgumentNullException(nameof(data), "Null data encountered");
    }
}
