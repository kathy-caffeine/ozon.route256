using Microsoft.Extensions.Configuration;

namespace PriceCalculator.ConsoleApp.Models;

internal class AppOptions
{
    public int MaxThreadsCount { get; set; }
    public int BufferSizeInLines { get; set; }

    // Возвращает настройки по умолчанию
    public static AppOptions Default()
    {
        return new AppOptions
        {
            MaxThreadsCount = 4,
            BufferSizeInLines = 1000
        };
    }

    public static List<string> Validate(AppOptions? appOptions)
    {
        List<string> validateProblems = new List<string>();

        if (appOptions == null)
        {
            validateProblems.Add("Конфигурация не можеть быть равна null");
            return validateProblems;
        }

        if (appOptions.MaxThreadsCount <= 0)
            validateProblems.Add("Количество потоков не может быть меньше или равно 0");

        if (appOptions.BufferSizeInLines <= 0)
            validateProblems.Add("Размер буфера не может быть меньше или равен 0");

        return validateProblems;
    }

    public static AppOptions GetOptions(IConfiguration configuration)
    {
        var appOptions = configuration.GetRequiredSection(nameof(AppOptions)).Get<AppOptions>()!;
        var validateResults = Validate(appOptions);

        if (validateResults.Count != 0)
        {
            foreach (var validateResult in validateResults)
            {
                Console.WriteLine(validateResult);
            }

            appOptions = Default();
        }


        return appOptions;
    }
}