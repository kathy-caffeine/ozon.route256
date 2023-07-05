using MediatR;
using Route256.Week5.Homework.PriceCalculator.Bll.Exceptions;
using Route256.Week5.Homework.PriceCalculator.Bll.Services.Interfaces;
using Route256.Week5.Homework.PriceCalculator.Dal.Entities;
using System.Text;

namespace Route256.Week5.Homework.PriceCalculator.Bll.Commands;

public record ClearCalculationHistoryCommand(
    long UserId,
    long[] CalculationsIds)
    : IRequest;

public class ClearCalculationHistoryCommandHandler : IRequestHandler<ClearCalculationHistoryCommand>
{
    private readonly ICalculationService _calculationService;

    public ClearCalculationHistoryCommandHandler(
        ICalculationService calculationService)
    {
        _calculationService = calculationService;
    }
    public async Task Handle(ClearCalculationHistoryCommand request, 
        CancellationToken cancellationToken)
    {
        // если есть массив индексов и нужно почистить только их
        if(request.CalculationsIds.Any())
        {
            // проверяем что все таски существуют
            var idsFlag = await _calculationService
                .CheckCalculationsIds(request.CalculationsIds, cancellationToken);
            if (idsFlag)
            {
                throw new OneOrManyCalculationsNotFoundException();
            }

            // проверяем, что все таски только одного юзера
            var usersFlag = await _calculationService.CheckUserIds(request.CalculationsIds, cancellationToken);
            var userCheck = usersFlag.GroupBy(x => x).ToArray();
            if (userCheck.Length > 1)
            {
                var sb = new StringBuilder("wrong_calculation_ids: ");
                foreach(var id in userCheck[1].ToArray())
                {
                    sb.Append(id.ToString() + ", ");
                }
                
                throw new OneOrManyCalculationsBelongsToAnotherUserException(sb.ToString());
            }

            // если всё ок
            await _calculationService.DeleteRequestEdentries(request.UserId, request.CalculationsIds,
                cancellationToken);
            return;
        }
        // если массив id пустой
        await _calculationService.DeleteRequestEdentries(request.UserId,
            cancellationToken);
        return;

    }
}
