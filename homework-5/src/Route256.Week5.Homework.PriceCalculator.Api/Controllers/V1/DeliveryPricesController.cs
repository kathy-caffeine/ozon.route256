using MediatR;
using Microsoft.AspNetCore.Mvc;
using Route256.Week5.Homework.PriceCalculator.Api.Requests.V1;
using Route256.Week5.Homework.PriceCalculator.Api.Responses.V1;
using Route256.Week5.Homework.PriceCalculator.Bll.Commands;
using Route256.Week5.Homework.PriceCalculator.Bll.Models;
using Route256.Week5.Homework.PriceCalculator.Bll.Queries;

namespace Route256.Week5.Homework.PriceCalculator.Api.Controllers.V1;

[ApiController]
[Route("/v1/delivery-prices")]
public class DeliveryPricesController : ControllerBase
{
    private readonly IMediator _mediator;

    public DeliveryPricesController(
        IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// Метод расчета стоимости доставки на основе объема товаров
    /// или веса товара. Окончательная стоимость принимается как наибольшая
    /// </summary>
    /// <returns></returns>
    [HttpPost("calculate")]
    public async Task<CalculateResponse> Calculate(
        CalculateRequest request,
        CancellationToken ct)
    {
        var command = new CalculateDeliveryPriceCommand(
            request.UserId,
            request.Goods
                .Select(x => new GoodModel(
                    x.Height,
                    x.Length,
                    x.Width,
                    x.Weight))
                .ToArray());
        var result = await _mediator.Send(command, ct);
        
        return new CalculateResponse(
            result.CalculationId,
            result.Price);
    }
    
    
    /// <summary>
    /// Метод получения истории вычисления
    /// </summary>
    /// <returns></returns>
    [HttpPost("get-history")]
    public async Task<GetHistoryResponse[]> History(
        GetHistoryRequest request,
        CancellationToken ct)
    {
        var query = new GetCalculationHistoryQuery(
            request.UserId,
            request.Take,
            request.Skip);
        var result = await _mediator.Send(query, ct);

        return result.Items
            .Select(x => new GetHistoryResponse(
                new GetHistoryResponse.CargoResponse(
                    x.Volume,
                    x.Weight,
                    x.GoodIds),
                x.Price))
            .ToArray();
    }


    /// <summary>
    /// Метод, очищающий для пользователя с заданным идентификатором
    /// рассчёты с идентификаторами из массива.
    /// 403 если если есть таски другого юзера
    /// 400 если есть несуществующие таски
    /// </summary>
    /// <param name="request"> содержит id пользователя, для которого удаляем рассчёты, 
    /// и массив id рассчётов. 
    /// массив может быть пустым, тогда нужно удалить все рассчёты пользователя. </param>
    /// <param name="ct"> Cancellation token для медиатора. </param>
    [HttpPost("clear-history")]
    public async Task ClearHistory(
        ClearHistoryRequest request,
        CancellationToken ct)
    {
        var command = new ClearCalculationHistoryCommand(
            request.UserId,
            request.CalculationsIds);
        await _mediator.Send(command, ct);
        return;
    }
}