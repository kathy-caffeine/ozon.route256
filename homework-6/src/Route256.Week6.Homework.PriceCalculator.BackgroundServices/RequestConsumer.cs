using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Hosting;
using Route256.Week5.Workshop.PriceCalculator.Bll.Commands;
using Route256.Week5.Workshop.PriceCalculator.Bll.Models;
using Route256.Week6.Homework.PriceCalculator.Bll.Kafka;
using Route256.Week6.Homework.PriceCalculator.Bll.Kafka.Models;

namespace Route256.Week6.Homework.PriceCalculator.BackgroundServices;

public class RequestConsumer : BackgroundService
{
    private readonly IMediator _mediator;

    public RequestConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task Consume(CancellationToken cts)
    {
        using var consumer = new ConsumerBuilder<Ignore, RequestMessage>(
            new ConsumerConfig{
                BootstrapServers = "localhost:9092",
                GroupId = "price_calc",
                EnableAutoCommit = true,
                EnableAutoOffsetStore = false
            })
            .SetValueDeserializer(new JsonValueSerializer<RequestMessage>())
            .Build();
        consumer.Subscribe("good_price_calc_requests");

        while (cts.IsCancellationRequested == false)
        {
            var result = consumer.Consume();
            if (result == null)
            {
                await Task.Delay(TimeSpan.FromSeconds(10), cts);
                continue;
            }

            var msg = result.Message.Value;

            var good = new GoodModel(
                msg.Height,
                msg.Length,
                msg.Width,
                msg.Weight);

            // валидация fluent валидатором

            await _mediator.Send(new CalculatePriceCommand(msg.GoodId, good), cts);
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Consume(stoppingToken);
    }
}