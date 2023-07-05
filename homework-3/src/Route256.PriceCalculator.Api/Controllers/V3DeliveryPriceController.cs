using Microsoft.AspNetCore.Mvc;
using Route256.PriceCalculator.Api.Requests.V3;
using Route256.PriceCalculator.Api.Responses.V3;
using Route256.PriceCalculator.Domain.Bll.Models.PriceCalculator;
using Route256.PriceCalculator.Domain.Bll.Services.Interfaces;
using CalculateRequest = Route256.PriceCalculator.Api.Requests.V3.CalculateRequest;

namespace Route256.PriceCalculator.Api.Controllers;

public class V3DeliveryPriceController: Controller
{
    private readonly IGoodPriceCalculatorService _goodPriceCalculatorService;
    private readonly IPriceCalculatorService _priceCalculatorService;

    public V3DeliveryPriceController(
        IGoodPriceCalculatorService goodPriceCalculatorService,
        IPriceCalculatorService priceCalculatorService)
    {
        _goodPriceCalculatorService = goodPriceCalculatorService;
        _priceCalculatorService = priceCalculatorService;
    }

    [HttpPost("calculate")]
    public CalculateResponse Calculate(
        CalculateRequest request)
    {
        var price = _priceCalculatorService.CalculatePrice(
                request.Goods
                    .Select(x => new GoodModel(
                        x.Height,
                        x.Length,
                        x.Width,
                        x.Weight))
                    .ToList());

        return new CalculateResponse(price);
    }
    
    [HttpPost("good/calculate")]
    public Task<CalculateResponse> Calculate(GoodCalculateRequest request)
    {
        var price = _goodPriceCalculatorService.CalculatePrice(request.GoodId, request.Distance);

        return Task.FromResult(new CalculateResponse(price));
    }

}