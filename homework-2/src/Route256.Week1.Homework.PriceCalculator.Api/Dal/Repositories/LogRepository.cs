using Route256.Week1.Homework.PriceCalculator.Api.Dal.Entities;
using Route256.Week1.Homework.PriceCalculator.Api.Dal.Repositories.Interfaces;

namespace Route256.Week1.Homework.PriceCalculator.Api.Dal.Repositories;

public class LogRepository : ILogRepository
{
    private readonly List<LogEntity> _log;

    public LogRepository()
    {
        _log = new List<LogEntity>();
    }

    public void AddLog(LogEntity logEntity) => _log.Add(logEntity);

    public List<LogEntity> GetAll() => _log;
}
