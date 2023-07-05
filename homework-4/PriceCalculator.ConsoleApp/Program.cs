using Microsoft.Extensions.Configuration;
using PriceCalculator.ConsoleApp.Models;
using PriceCalculator.ConsoleApp;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var (inputFilePath, outputFilePath) = ConsoleUtils.GetIOFilePaths();

var concurrentFileProcessor = new ConcurrentFileProcessor(configuration);
concurrentFileProcessor.ProcessFile(inputFilePath, outputFilePath);