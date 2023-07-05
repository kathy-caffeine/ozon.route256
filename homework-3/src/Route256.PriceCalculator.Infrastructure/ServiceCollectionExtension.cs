using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Route256.PriceCalculator.Api.Dal.Repositories;
using Route256.PriceCalculator.Domain.Bll;
using Route256.PriceCalculator.Domain.Bll.Services.Interfaces;
using Route256.PriceCalculator.Domain.Separated;
using Route256.PriceCalculator.Infrastructure.Dal.Repositories;
using Route256.PriceCalculator.Infrastructure.External;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Route256.PriceCalculator.Infrastructure;

public static class ServiceCollectionExtension
{
    
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IStorageRepository, StorageRepository>();
        services.AddSingleton<IGoodsRepository, GoodsRepository>();
        services.AddScoped<IGoodsService, GoodsService>();
        return services;
    }
}
