using Route256.Week1.Homework.PriceCalculator.Api.Dal.Entities;

namespace Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.Interfaces;

public interface IGoodPriceCalculationService
{
    /// <summary>
    /// Метод подсчёта полной стоимости товара:
    /// цена + стоимость доставки с учётом расстояния
    /// </summary>

    decimal CalculatePrice(GoodEntity good, int distance);
}
