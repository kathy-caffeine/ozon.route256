using Route256.PriceCalculator.Domain.Bll.Services.Interfaces;
using Route256.PriceCalculator.Domain.Bll.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Route256.PriceCalculator.Domain.Bll;

namespace Route256.PriceCalculator.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IPriceCalculatorService, PriceCalculatorService>();
        services.AddScoped<IGoodPriceCalculatorService, GoodPriceCalculatorService>();
        services.Configure<PriceCalculatorOptions>(configuration.GetSection("PriceCalculatorOptions"));
        return services;
    }
}
