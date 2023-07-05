using Route256.Week1.Homework.PriceCalculator.Api.Bll.Models.PriceCalculator;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.Interfaces;
using Route256.Week1.Homework.PriceCalculator.Api.Dal.Entities;
using Route256.Week1.Homework.PriceCalculator.Api.Dal.Repositories.Interfaces;

namespace Route256.Week1.Homework.PriceCalculator.Api.Bll.Services;

public class GetStatisticsService : IGetStatisticsService
{
    private readonly IStorageRepository _storageRepository;
    public GetStatisticsService(
        IStorageRepository storageRepository)
    {
        _storageRepository = storageRepository;
    }
    

    private (decimal Weigth, int Distance) GetWeightStatistics(IReadOnlyList<StorageEntity> query)
    {
        var maxWeightEntity = query.OrderBy(x => x.Weight).Last();

        return (maxWeightEntity.Weight, maxWeightEntity.Distance);
    }

    private (decimal Volume, int Distance) GetVolumeStatistics(IReadOnlyList<StorageEntity> query)
    {
        var maxVolumeEntity = query.OrderBy(x => x.Volume).Last();

        return (maxVolumeEntity.Volume, maxVolumeEntity.Distance);
    }

    public Statistics GetStatistics()
    {
        var log = _storageRepository.Query();

        decimal wavg_price =
            log.Sum(x => x.Price * x.Amount)
            / log.Sum(x => x.Amount);
        var weight = GetWeightStatistics(log);
        var volume = GetVolumeStatistics(log);

        return new Statistics(
            (int)weight.Weigth,
            (int)volume.Volume,
            weight.Distance,
            volume.Distance,
            wavg_price
            );
    }
}
