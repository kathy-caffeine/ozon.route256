using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using PriceCalculator.ConsoleApp.Models;
using PriceCalculator.ConsoleApp.Services;
using System.Collections.Concurrent;

namespace PriceCalculator.ConsoleApp;

internal class ConcurrentFileProcessor
{
    private AppOptions _appOptions;
    private readonly object _writeFileLocker = new();
    private readonly object _readFileLocker = new();
    private readonly object _processedLinesLocker = new();
    private readonly IConfiguration _configuration;
    private IChangeToken _configChangeToken;
    private int _wroteLines;
    private int _readLines;
    private int _processedLines = 1;

    public ConcurrentFileProcessor(IConfiguration configuration)
    {
        _appOptions = AppOptions.GetOptions(configuration);
        _configuration = configuration;
        _configChangeToken = _configuration.GetReloadToken();
    }

    public void ProcessFile(string inputFileName, string outputFileName)
    {
        long offset = 0;
        var currentPart = ReadPartFromFile(inputFileName, ref offset);

        File.WriteAllText(outputFileName, "id, price\n");

        while (currentPart != null && currentPart.Count != 0)
        {
            if (_configChangeToken.HasChanged)
            {
                _appOptions = AppOptions.GetOptions(_configuration);
                Console.WriteLine($"Текущее количество потоков: {_appOptions.MaxThreadsCount}");
                Console.WriteLine($"Текущий размер буффера: {_appOptions.BufferSizeInLines}");
                _configChangeToken = _configuration.GetReloadToken();
            }

            var processedPart = ProcessPart(currentPart);
            WriteProcessedPartToFile(outputFileName, processedPart);
            currentPart = ReadPartFromFile(inputFileName, ref offset);
        }
    }

    ConcurrentBag<string> ProcessPart(IReadOnlyCollection<string> currentPart)
    {
        var processedLines = new ConcurrentBag<string>();

        Parallel.ForEach(currentPart, new ParallelOptions
            {
                MaxDegreeOfParallelism = _appOptions.MaxThreadsCount
            },
            line =>
            {
                processedLines.Add(
                    ProcessLine(line) + Environment.NewLine
                );
            });

        return processedLines;
    }

    private string ProcessLine(string line)
    {
        Thread.Sleep(2500);
        var lst = line.Split(", ");
        var res = new GoodParams(
            Convert.ToInt32(lst[0]),
            Convert.ToInt32(lst[1]),
            Convert.ToInt32(lst[2]),
            Convert.ToInt32(lst[3]),
            Convert.ToInt32(lst[4])
        );
        var price = PriceCalculatorService.CalculatePrice(res, 1000);

        // Тут нужно распарсить строку вида "123, 2, 1, 1, 10"
        // Посчитать сумму и вернуть ее тоже в виде строки
        lock (_processedLinesLocker)
        {
            Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} обработал {_processedLines} строку");
            _processedLines++;
        }

        return "" + res.id + " " + price;
    }

    void WriteProcessedPartToFile(string outputFileName, ConcurrentBag<string> processedData)
    {
        lock (_writeFileLocker)
        {
            using (StreamWriter writer = new StreamWriter(outputFileName, true))
            {
                writer.Write(processedData.Aggregate((x, y) => x + y));
            }

            _wroteLines += processedData.Count;
            Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} записал {processedData.Count} строк");
            Console.WriteLine($"Итоговое количество записанных строк: {_wroteLines}");
        }
    }

    List<string> ReadPartFromFile(string inputFileName, ref long offset)
    {
        lock (_readFileLocker)
        {
            List<string> buffer = new(_appOptions.BufferSizeInLines);

            using (var stream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read))
            using (StreamReader reader = new StreamReader(stream))
            {
                // Пропускаем первую строку
                if (offset == 0)
                {
                    var firstLine = reader.ReadLine();
                    if (firstLine == null) return buffer;
                    offset = firstLine.Length + Environment.NewLine.Length;
                    //offset = reader.BaseStream.Position;
                }

                stream.Seek(offset, SeekOrigin.Begin);
                for (int i = 0; i < _appOptions.BufferSizeInLines; i++)
                {
                    var line = reader.ReadLine();
                    if (line == null) break;
                    offset += line.Length + Environment.NewLine.Length;
                    buffer.Add(line);
                    _readLines++;
                    Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} прочитал строку №" + _readLines);
                }
            }

            return buffer;
        }
    }
}