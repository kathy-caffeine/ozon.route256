using Microsoft.AspNetCore.Mvc;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Models.PriceCalculator;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.Interfaces;
using Route256.Week1.Homework.PriceCalculator.Api.Requests.V3;
using Route256.Week1.Homework.PriceCalculator.Api.Responses.V3;

namespace Route256.Week1.Homework.PriceCalculator.Api.Controllers;

[ApiController]
[Route("/v3/[controller]")]
public class V3DeliveryPriceController : ControllerBase
{
    private readonly IPriceCalculatorService _priceCalculatorService;
    private readonly IGetStatisticsService _getStatisticsService;

    public V3DeliveryPriceController(
        IPriceCalculatorService priceCalculatorService,
        IGetStatisticsService getStatisticsService)
    {
        _priceCalculatorService = priceCalculatorService;
        _getStatisticsService = getStatisticsService;
    }
    
    /// <summary>
    /// Метод расчета стоимости доставки на основе объема товаров
    /// или веса товара. Окончательная стоимость принимается как наибольшая
    /// Учитывается расстояние, на которое необходимо перевезти товары.
    /// </summary>
    /// <returns></returns>
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
                .ToArray(), request.Distance);

        return new CalculateResponse(price);
    }

    /// <summary>
    /// Метод получения истории рассчёта параметров товара
    /// </summary>
    /// <returns></returns>
    [HttpPost("get-history")]
    public GetHistoryResponse[] GetHistory(GetHistoryRequest request)
    {
        var log = _priceCalculatorService.QueryLog(request.Take);

        return log
            .Select(x => new GetHistoryResponse(
                new CargoResponse(
                    x.Volume,
                    x.Weight),
                x.Price,
                x.Distance))
            .ToArray();
    }

    /// <summary>
    /// Метод очистки истории вычислений параметров товара
    /// </summary>
    /// <returns></returns>
    [HttpPost("clear-history")]
    public ClearHistoryResponse ClearHistory(ClearHistoryRequest request)
    {
        _priceCalculatorService.ClearLog();

        return new ClearHistoryResponse("История успешно очищена.");
    }

    /// <summary>
    /// Метод получения статистических данных о наборе товаров, таких как
    /// максимальный вес, максимальный объем, 
    /// расстояние, на которое перевезли товары с максимальными метриками
    /// и средневзвешенная стоимость доставки.
    /// </summary>
    /// <returns></returns>
    [HttpPost("get-statistics")]
    public StatisticsResponse GetStatistics(StatisticsRequest request)
    {
        var res = _getStatisticsService.GetStatistics();

        return new StatisticsResponse(
           res.max_weight, res.max_volume, res.max_distance_for_heaviest_good,
           res.max_distance_for_largest_good, res.wavg_price);
    }
}