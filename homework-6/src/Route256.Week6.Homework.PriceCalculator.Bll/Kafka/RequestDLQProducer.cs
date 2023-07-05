using Confluent.Kafka;
using Route256.Week6.Homework.PriceCalculator.Bll.Kafka.Models;

namespace Route256.Week6.Homework.PriceCalculator.Bll.Kafka;

public class RequestDLQProducer
{
    const string topic = "good_price_calc_requests_dlq";

    public async Task Produce(RequestMessage message)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromMinutes(1));
        using var producer = new ProducerBuilder<Ignore, RequestMessage>(
            new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                Acks = Acks.All
            })
            .SetValueSerializer(new JsonValueSerializer<RequestMessage>())
            .Build();
        await producer.ProduceAsync(topic,
            new Message<Ignore, RequestMessage>
            {
                Value = message
            },
            cts.Token);

        producer.Flush();
    }
}
