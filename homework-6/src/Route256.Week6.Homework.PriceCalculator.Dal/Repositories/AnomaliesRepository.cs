using Dapper;
using Microsoft.Extensions.Options;
using Route256.Week5.Workshop.PriceCalculator.Dal.Entities;
using Route256.Week5.Workshop.PriceCalculator.Dal.Models;
using Route256.Week5.Workshop.PriceCalculator.Dal.Repositories;
using Route256.Week5.Workshop.PriceCalculator.Dal.Repositories.Interfaces;
using Route256.Week5.Workshop.PriceCalculator.Dal.Settings;
using Route256.Week6.Homework.PriceCalculator.Dal.Entities;

namespace Route256.Week6.Homework.PriceCalculator.Dal.Repositories;

internal class AnomaliesRepository : BaseRepository, IAnomaliesRepository
{
    public AnomaliesRepository(IOptions<DalOptions> dalSettings) : base(dalSettings.Value)
    { }

    public async Task Add(AnomaliesEntityV1 entityV1, CancellationToken token)
    {
        const string sqlQuery = @"
insert into anomalies (good_id, price)
@good_id, @price
";

        var sqlQueryParams = new
        {
            good_id = entityV1.GoodId,
            price = entityV1.price
        };

        await using var connection = await GetAndOpenConnection();
        await connection.QueryAsync<long>(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams,
                cancellationToken: token));
        return;
    }

    public Task<CalculationEntityV1[]> Query(CalculationHistoryQueryModel query, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}
