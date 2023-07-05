using Microsoft.Extensions.Options;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.Interfaces;
using Route256.Week1.Homework.PriceCalculator.Api.Dal.Repositories.Interfaces;

namespace Route256.Week1.Homework.PriceCalculator.Api.HostedServices;

public sealed class GoodsSyncHostedService: BackgroundService
{
    private readonly IGoodsRepository _repository;
    private readonly IServiceProvider _serviceProvider;
    private readonly IOptionsMonitor<UpdateOptions> _updateOptionsMonitor;

    public GoodsSyncHostedService(
        IGoodsRepository repository,
        IServiceProvider serviceProvider,
        IOptionsMonitor<UpdateOptions> updateOptionsMonitor)
    {
        _repository = repository;
        _serviceProvider = serviceProvider;
        _updateOptionsMonitor = updateOptionsMonitor;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var goodsService = scope.ServiceProvider.GetRequiredService<IGoodsService>();
                var goods = goodsService.GetGoods().ToList();
                foreach (var good in goods)
                    _repository.AddOrUpdate(good);
            }
            
            await Task.Delay(TimeSpan.FromSeconds(_updateOptionsMonitor.CurrentValue.UpdateCyclePeriod), stoppingToken);
        }
    }
}