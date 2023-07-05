using Microsoft.AspNetCore.Mvc;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Models.PriceCalculator;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Services;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.Interfaces;
using Route256.Week1.Homework.PriceCalculator.Api.Dal.Entities;
using Route256.Week1.Homework.PriceCalculator.Api.Dal.Repositories.Interfaces;
using Route256.Week1.Homework.PriceCalculator.Api.Requests.V1;
using Route256.Week1.Homework.PriceCalculator.Api.Responses.V1;

namespace Route256.Week1.Homework.PriceCalculator.Api.Controllers;


[Route("v1/goods")]
[ApiController]
public sealed class V1GoodsController
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<V1GoodsController> _logger;
    private readonly IGoodsRepository _repository;
    private readonly ILogRepository _logRepository;

    public V1GoodsController(
        IHttpContextAccessor httpContextAccessor,
        ILogger<V1GoodsController> logger,
        IGoodsRepository repository,
        ILogRepository logRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _repository = repository;
        _logRepository = logRepository;
    }

    /// <summary>
    /// «апрос дл€ получени€ списка всех товаров
    /// </summary>

    [HttpGet]
    public ICollection<GoodEntity> GetAll()
    {
        return _repository.GetAll();
    }

    /// <summary>
    /// «апрос дл€ получени€ стоимости доставки товара по его id
    /// </summary>

    [HttpGet("calculate/{id}")]
    public CalculateResponse Calculate(
        [FromServices] IPriceCalculatorService priceCalculatorService,
        int id)
    {
        _logger.LogInformation(_httpContextAccessor.HttpContext.Request.Path);

        var good = _repository.Get(id);
        var model = new GoodModel(
            good.Height,
            good.Length,
            good.Width,
            good.Weight);

        var price = priceCalculatorService.CalculatePrice(new[] { model });
        return new CalculateResponse(price);
    }

    /// <summary>
    /// «апрос дл€ получени€ полной стоимости товара, то есть
    /// сумму цены за товар и доставки
    /// </summary>
    [HttpPost("calculate")]
    public CalculateResponse Calculate(
        GoodCalculateRequest request,
        [FromServices] IGoodPriceCalculationService _goodPriceCalculationService)
    {
        return new CalculateResponse(
            _goodPriceCalculationService
            .CalculatePrice(_repository.Get(request.id), request.distance));
    }


    /// <summary>
    /// «апрос дл€ получени€ истории запросов
    /// </summary>
    [HttpPost("get-all")]
    public GetLogResponse GetLog()
    {
        return new GetLogResponse(_logRepository.GetAll().ToArray());
    }
}