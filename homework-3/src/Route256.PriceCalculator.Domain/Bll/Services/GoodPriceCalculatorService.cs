using Route256.PriceCalculator.Domain.Bll.Models.PriceCalculator;
using Route256.PriceCalculator.Domain.Bll.Services.Interfaces;
using Route256.PriceCalculator.Domain.Entities;
using Route256.PriceCalculator.Domain.Separated;

namespace Route256.PriceCalculator.Domain.Bll.Services;

internal sealed class GoodPriceCalculatorService : IGoodPriceCalculatorService
{
    private readonly IGoodsRepository _repository;
    private readonly IPriceCalculatorService _service;

    public GoodPriceCalculatorService(
        IGoodsRepository repository,
        IPriceCalculatorService service)
    {
        this._repository = repository;
        this._service = service;
    }
    
    public decimal CalculatePrice(
        int id, 
        decimal distance)
    {
        if (id == default)
            throw new ArgumentException($"{nameof(id)} is default");
        
        if (distance == default)
            throw new ArgumentException($"{nameof(distance)} is default");

        var requestedGood = _repository.Get(id);
        var goodModel = new GoodModel(
            requestedGood.Height,
            requestedGood.Length,
            requestedGood.Width,
            requestedGood.Weight); 

        var price = _service.CalculatePrice(
            new List<GoodModel>{ goodModel } ) 
            * distance;

        return price;
    }
}