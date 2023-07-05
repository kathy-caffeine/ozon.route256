using Route256.Week1.Homework.PriceCalculator.Api.Bll.Models.PriceCalculator;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.Interfaces;
using Route256.Week1.Homework.PriceCalculator.Api.Dal.Entities;
using Route256.Week1.Homework.PriceCalculator.Api.Dal.Repositories.Interfaces;

namespace Route256.Week1.Homework.PriceCalculator.Api.Bll.Services;

public class PriceCalculatorService : IPriceCalculatorService
{
    private const decimal volumeToPriceRatio = 3.27m;
    private const decimal weightToPriceRatio = 1.34m;
    
    private readonly IStorageRepository _storageRepository;
    
    public PriceCalculatorService(
        IStorageRepository storageRepository)
    {
        _storageRepository = storageRepository;
    }
    
    public decimal CalculatePrice(IReadOnlyList<GoodModel> goods, int distance)
    {
        if (!goods.Any())
        {
            throw new ArgumentOutOfRangeException(nameof(goods));
        }

        var volumePrice = CalculatePriceByVolume(goods, out var volume);
        var weightPrice = CalculatePriceByWeight(goods, out var weight);

        var resultPrice = Math.Max(volumePrice, weightPrice)*distance/1000;
        
        _storageRepository.Save(new StorageEntity(
            DateTime.UtcNow,
            volume,
            weight,
            resultPrice,
            distance,
            goods.Count));
        
        return resultPrice;
    }

    private decimal CalculatePriceByVolume(
        IReadOnlyList<GoodModel> goods,
        out decimal volume)
    {
        volume = goods
            .Select(x => x.Height * x.Width * x.Height)
            .Sum();

        return volume * volumeToPriceRatio;
    }
    
    private decimal CalculatePriceByWeight(
        IReadOnlyList<GoodModel> goods,
        out decimal weight)
    {
        weight = goods
            .Select(x => x.Weight)
            .Sum();

        return weight * weightToPriceRatio;
    }

    public CalculationLogModel[] QueryLog(int take)
    {
        if (take == 0)
        {
            return Array.Empty<CalculationLogModel>();
        }
        
        var log = _storageRepository.Query()
            .OrderByDescending(x => x.At)
            .Take(take)
            .ToArray();

        return log
            .Select(x => new CalculationLogModel(
                x.Volume, 
                x.Weight,
                x.Price,
                x.Distance))
            .ToArray();
    }

    public void ClearLog()
    {
        _storageRepository.Clear();
    }

    public void GetStatistics(out int max_weight, out int max_volume, 
        out int max_distance_for_heaviest_good, out int max_distance_for_largest_good, 
        out decimal wavg_price)
    {
        var log = _storageRepository.Query();

        (max_weight, max_distance_for_heaviest_good) = GetWeightStatistics(log);

        (max_volume, max_distance_for_largest_good) = GetVolumeStatistics(log);

        wavg_price = 
            log.Sum(x => x.Price)
            / log.Sum(x=>x.Amount) ;
    }

    private (int Weigth, int Distance) GetWeightStatistics(IReadOnlyList<StorageEntity> query)
    {
        var maxWeightEntity = query.OrderBy(x => x.Weight).Last();

        return ((int)maxWeightEntity.Weight, maxWeightEntity.Distance);
    }


    private (int Volume, int Distance) GetVolumeStatistics(IReadOnlyList<StorageEntity> query)
    {
        var maxVolumeEntity = query.OrderBy(x => x.Volume).Last();

        return ((int)maxVolumeEntity.Volume, maxVolumeEntity.Distance);
    }

}
