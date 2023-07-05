using Route256.Week5.Homework.PriceCalculator.Bll.Models;

namespace Route256.Week5.Homework.PriceCalculator.Bll.Services.Interfaces;

public interface ICalculationService
{
    Task<long> SaveCalculation(
        SaveCalculationModel saveCalculation,
        CancellationToken cancellationToken);

    decimal CalculatePriceByVolume(
        GoodModel[] goods,
        out double volume);

    public decimal CalculatePriceByWeight(
        GoodModel[] goods,
        out double weight);

    Task<QueryCalculationModel[]> QueryCalculations(
        QueryCalculationFilter query,
        CancellationToken token);

    public Task<long[]> CheckUserIds(long[] ids,
        CancellationToken cancellationToken);

    public Task<bool> CheckCalculationsIds(long[] ids,
        CancellationToken cancellationToken);

   /* public Task DeleteUsersIds(long[] ids,
        CancellationToken cancellationToken);*/
    public Task DeleteRequestEdentries(long userId, long[] ids,
        CancellationToken cancellationToken);

    public Task DeleteRequestEdentries(long userId,
        CancellationToken cancellationToken);
}