using Dapper;
using Microsoft.Extensions.Options;
using Route256.Week5.Homework.PriceCalculator.Dal.Entities;
using Route256.Week5.Homework.PriceCalculator.Dal.Models;
using Route256.Week5.Homework.PriceCalculator.Dal.Repositories.Interfaces;
using Route256.Week5.Homework.PriceCalculator.Dal.Settings;

namespace Route256.Week5.Homework.PriceCalculator.Dal.Repositories;

public class CalculationRepository : BaseRepository, ICalculationRepository
{
    public CalculationRepository(
        IOptions<DalOptions> dalSettings) : base(dalSettings.Value)
    {
    }
    
    public async Task<long[]> Add(
        CalculationEntityV1[] entityV1, 
        CancellationToken token)
    {
        const string sqlQuery = @"
insert into calculations (user_id, good_ids, total_volume, total_weight, price, at)
select user_id, good_ids, total_volume, total_weight, price, at
  from UNNEST(@Calculations)
returning id;
";
        
        var sqlQueryParams = new
        {
            Calculations = entityV1
        };
        
        await using var connection = await GetAndOpenConnection();
        var ids = await connection.QueryAsync<long>(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams,
                cancellationToken: token));
        
        return ids
            .ToArray();
    }

    public async Task<CalculationEntityV1[]> Query(
        CalculationHistoryQueryModel query,
        CancellationToken token)
    {
        const string sqlQuery = @"
select id
     , user_id
     , good_ids
     , total_volume
     , total_weight
     , price
     , at
  from calculations
 where user_id = @UserId
 order by at desc
 limit @Limit offset @Offset
";
        
        var sqlQueryParams = new
        {
            UserId = query.UserId,
            Limit = query.Limit,
            Offset = query.Offset
        };

        await using var connection = await GetAndOpenConnection();
        var calculations = await connection.QueryAsync<CalculationEntityV1>(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams,
                cancellationToken: token));
        
        return calculations
            .ToArray();
    }

    public async Task<long[]> GetUserIds(
        long[] CalcIds,
        CancellationToken token)
    {
        const string sqlQuery = @"
select user_id
from calculations
where id in(
select * from UNNEST(@ids))
";

        var sqlQueryParams = new
        {
            ids = CalcIds,
        };

        await using var connection = await GetAndOpenConnection();
        var ids = await connection.QueryAsync<long>(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams,
                cancellationToken: token));

        return ids.ToArray();
    }

    public async Task<bool> GetNotExistedIds(
        long[]? CalcIds, 
        CancellationToken token)
    {
        const string sqlQuery = @"
select t.id
from (
  select unnest(@ids) as id
) as t
left join calculations c on c.id = t.id
where c.id is null
";

        var sqlQueryParams = new
        {
            ids = CalcIds,
        };

        await using var connection = await GetAndOpenConnection();
        var ids = await connection.QueryAsync<long>(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams,
                cancellationToken: token));

        return ids.ToArray().Length>0;

    }

    public async Task DeleteRequestedEntries(long userId, long[] calcIds,
       CancellationToken token)
    {
        // удаляем вычисления и возвращаем id удалённых вычислений
        const string sqlQuery = @"
create temp table goods_ids
(
    id int8 not null
)  on commit drop;

insert into goods_ids (id)
select goods
from calculations c, unnest(c.good_ids) as goods
where c.user_id = @user_id and c.id in (
select * from unnest (@ids));

delete from calculations c where c.user_id = @user_id and c.id in 
(select * from unnest (@ids));

delete from goods using goods_ids where goods.id = goods_ids.id;
";
        var sqlQueryParams = new
        {
            user_id = userId,
            ids = calcIds,
        };
        await using var connection = await GetAndOpenConnection();
        var idsGoodsToDelete = await connection.QueryAsync<long>(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams,
                cancellationToken: token));

        return;
    }

    public async Task DeleteRequestedEntries(long userId, 
       CancellationToken token)
    {
        // удаляем вычисления и возвращаем id удалённых вычислений
        const string sqlQuery = @"
create temp table goods_ids
(
    id int8 not null
)  on commit drop;

insert into goods_ids (id)
select goods
from calculations c, unnest(c.good_ids) as goods
where c.user_id = @user_id;

delete from calculations c where c.user_id = @user_id;

delete from goods using goods_ids where goods.id = goods_ids.id;
";
        var sqlQueryParams = new
        {
            user_id = userId,
        };
        await using var connection = await GetAndOpenConnection();
        var idsGoodsToDelete = await connection.QueryAsync<long>(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams,
                cancellationToken: token));

        return;
    }
}