using Route256.Week5.Workshop.PriceCalculator.Dal.Entities;
using Route256.Week5.Workshop.PriceCalculator.Dal.Models;
using Route256.Week6.Homework.PriceCalculator.Dal.Entities;

namespace Route256.Week5.Workshop.PriceCalculator.Dal.Repositories.Interfaces;

public interface IAnomaliesRepository : IDbRepository
{
    Task Add(
        AnomaliesEntityV1 entityV1, 
        CancellationToken token);

    Task<CalculationEntityV1[]> Query(
        CalculationHistoryQueryModel query,
        CancellationToken token);
}