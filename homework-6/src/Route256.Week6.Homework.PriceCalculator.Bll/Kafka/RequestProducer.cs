using Confluent.Kafka;
using Route256.Week6.Homework.PriceCalculator.Bll.Kafka.Models;

namespace Route256.Week6.Homework.PriceCalculator.Bll.Kafka;

public class RequestProducer
{
    const string topic = "price_calc";

    public async Task Produce(OrderMessage message)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromMinutes(1));
        using var producer = new ProducerBuilder<Ignore, OrderMessage>(
            new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                Acks = Acks.All
            })
            .SetValueSerializer(new JsonValueSerializer<OrderMessage>())
            .Build();
        await producer.ProduceAsync(topic,
            new Message<Ignore, OrderMessage>
            {
                Value = message
            },
            cts.Token);

        producer.Flush();
    }
}
