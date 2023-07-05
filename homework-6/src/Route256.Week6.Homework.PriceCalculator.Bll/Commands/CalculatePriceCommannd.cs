using MediatR;
using Route256.Week5.Workshop.PriceCalculator.Bll.Models;
using Route256.Week5.Workshop.PriceCalculator.Bll.Services.Interfaces;
using Route256.Week6.Homework.PriceCalculator.Bll.Kafka;
using Route256.Week6.Homework.PriceCalculator.Bll.Kafka.Models;

namespace Route256.Week5.Workshop.PriceCalculator.Bll.Commands;

public record CalculatePriceCommand(
        long GoodId,
        GoodModel Good)
    : IRequest;

public class CalculatePriceCommandHandler 
    : IRequestHandler<CalculatePriceCommand>
{
    private readonly ICalculationService _calculationService;

    public CalculatePriceCommandHandler(
        ICalculationService calculationService)
    {
        _calculationService = calculationService;
    }
    
    public async Task Handle(
        CalculatePriceCommand request,
        CancellationToken cancellationToken)
    {
        var crunch = Validate(request.Good);
        if(crunch == false)
        {
            var DLQProducer = new RequestDLQProducer();
            await DLQProducer.Produce(new RequestMessage(
                request.GoodId, request.Good.Height, 
                request.Good.Length, request.Good.Width,
                request.Good.Weight));
            return;
        }
        var price = _calculationService.CalculatePrice(request.Good);
        var producer = new RequestProducer();
        await producer.Produce(new OrderMessage(
            request.GoodId,
            price));
    }

    private bool Validate(GoodModel good)
    {
        if(good == null) return false;
        if((good.Height > 0) 
            && (good.Width > 0) 
            && (good.Weight > 0) 
            && (good.Length > 0)) return true;
        return false;
    }
}