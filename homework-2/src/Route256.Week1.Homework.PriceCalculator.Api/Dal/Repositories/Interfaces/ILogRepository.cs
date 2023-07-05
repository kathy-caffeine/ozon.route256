using Route256.Week1.Homework.PriceCalculator.Api.Dal.Entities;

namespace Route256.Week1.Homework.PriceCalculator.Api.Dal.Repositories.Interfaces;

public interface ILogRepository
{
    /// <summary>
    /// Метод добавления запили в лог
    /// </summary>
    void AddLog(LogEntity logEntity);

    /// <summary>
    /// Метод получения всех записей в лог
    /// </summary>
    List<LogEntity> GetAll();
}
