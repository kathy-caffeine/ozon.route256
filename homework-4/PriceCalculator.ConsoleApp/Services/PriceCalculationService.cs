using PriceCalculator.ConsoleApp.Models;

namespace PriceCalculator.ConsoleApp.Services;

public class PriceCalculatorService
{
    private const decimal VolumeToPriceRatio = 3.27m;
    private const decimal WeightToPriceRatio = 1.34m;

    public static decimal CalculatePrice(GoodParams good, int distance)
    {
        var volumePrice = CalculatePriceByVolume(good);
        var weightPrice = CalculatePriceByWeight(good);

        var resultPrice = Math.Max(volumePrice, weightPrice) * distance / 1000;

        return resultPrice;
    }

    private static decimal CalculatePriceByVolume(
        GoodParams good)
    {
        var volume = good.height * good.width * good.length;

        return volume * VolumeToPriceRatio;
    }

    private static decimal CalculatePriceByWeight(
        GoodParams good)
    {
        return good.weight * WeightToPriceRatio;
    }
}