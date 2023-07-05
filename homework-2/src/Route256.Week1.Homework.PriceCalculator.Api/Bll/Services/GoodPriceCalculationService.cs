using Microsoft.Extensions.Options;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.Interfaces;
using Route256.Week1.Homework.PriceCalculator.Api.Dal.Entities;
using Route256.Week1.Homework.PriceCalculator.Api.Dal.Repositories.Interfaces;

namespace Route256.Week1.Homework.PriceCalculator.Api.Bll.Services;

public class GoodPriceCalculationService : IGoodPriceCalculationService
{
    private readonly IStorageRepository _storageRepository;
    public decimal _volumeToPriceRatio;
    public decimal _weightToPriceRatio;

    public GoodPriceCalculationService(
        IStorageRepository storageRepository,
        IOptionsSnapshot<PriceCalculatorOptions> options)
    {
        _storageRepository = storageRepository;
        _volumeToPriceRatio = options.Value.VolumeToPriceRatio;
        _weightToPriceRatio = options.Value.WeightToPriceRatio;
    }

    public decimal CalculatePrice(GoodEntity good,
        int distance)
    {
        var volumePrice = CalculatePriceByVolume(good, out var volume);
        var weightPrice = CalculatePriceByWeight(good, out var weight);

        var resultPrice = Math.Max(volumePrice, weightPrice) * distance / 1000;

        _storageRepository.Save(new StorageEntity(
            DateTime.UtcNow,
            volume,
            weight,
            resultPrice));

        return resultPrice;

    }

    private decimal CalculatePriceByVolume(
        GoodEntity good,
        out decimal volume)
    {
        volume = good.Height * good.Width * good.Height;

        return volume * _volumeToPriceRatio;
    }

    private decimal CalculatePriceByWeight(
        GoodEntity good,
        out decimal weight)
    {
        weight = good.Weight / 1000;

        return weight * _weightToPriceRatio;
    }
}
