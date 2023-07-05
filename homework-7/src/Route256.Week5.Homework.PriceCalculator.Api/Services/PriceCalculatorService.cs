using Grpc.Core;
using Route256.Week5.Homework.PriceCalculator.Bll.Models;
using Route256.Week5.Homework.PriceCalculator.Bll.Services.Interfaces;
using Route256.Week5.Homework.PriceCalculator.Grpc;

namespace Route256.Week5.Homework.PriceCalculator.Api.Services;

public class PriceCalculatorService : PriceCalculator.Grpc.PriceCalculator.PriceCalculatorBase
{
    ICalculationService _calculationService;

    public PriceCalculatorService(ICalculationService calculationService)
    {
        _calculationService = calculationService;
    }

    public override Task<DeliveryPriceResponse>
        CountDeliveryPrice(
        DeliveryPriceRequest request,
        ServerCallContext context)
    {
        var GoodList = new List<GoodModel>();
        foreach(var g in request.Goods)
        {
            GoodList.Add(
                new Route256.Week5.Homework.PriceCalculator.Bll.Models.GoodModel(
                    g.Height,
                    g.Length,
                    g.Width,
                    g.Weight));
        }
        var res = _calculationService.CalculatePrice(GoodList);

        return Task.FromResult(new DeliveryPriceResponse
        {
            Result = DecimalValue.FromDecimal(res)
        });
    }

    public override Task<Empty>
        DeleteUserHistory(
        DeleteUserHistoryRequest request,
        ServerCallContext context)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(120));
        _calculationService.DeleteRequestEdentries(request.UserId, request.GoodIds.ToArray(), cts.Token);
        return Task.FromResult(new Empty { });
    }

    public override async Task
        GetUserHistory(
        GetUserHistoryRequest request,
        IServerStreamWriter<HistoryResponse> responseStream,
        ServerCallContext context)
    {
        var filter = new QueryCalculationFilter(request.UserId, int.MaxValue, 0, null);
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(120));
        var history = await _calculationService.QueryCalculations(filter, cts.Token);
        foreach (var item in history)
        {
            await responseStream.WriteAsync(new HistoryResponse
            {
                GoodId = item.Id,
                Result = DecimalValue.FromDecimal(item.Price)
            });
        }
    }
}
