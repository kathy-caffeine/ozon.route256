using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceCalculator.ConsoleApp.Models;

internal class ConsoleUtils
{
    public static (string inputFilePath, string outputFilePath) GetIOFilePaths()
    {
        Console.WriteLine("Введите название файла из которого будет происходить чтение");
        var inputFilePath = GetFilePathFromConsole();
        Console.WriteLine($"Название входного файла: {inputFilePath}\n");

        Console.WriteLine("Введите название файла в который будет происходить запись полученного результата");
        string outputFilePath;

        do
        {
            outputFilePath = GetFilePathFromConsole();
            Console.WriteLine($"Название выходного файла: {outputFilePath}");
            if (inputFilePath == outputFilePath)
            {
                Console.WriteLine();
                Console.WriteLine("!!!Название выходного файла должно отличаться от входного!!!\n" +
                                  "Выберите другое название для выходного файла\n");
            }
        } while (inputFilePath == outputFilePath);

        return (inputFilePath, outputFilePath);
    }

    private static string GetFilePathFromConsole()
    {
        bool isCorrectInput = false;
        string filePathFromConsole;

        do
        {
            Console.Write("Название файла: ");
            filePathFromConsole = Console.ReadLine()!;

            if (!File.Exists(filePathFromConsole))
            {
                Console.WriteLine("Указанный файл не найден");
                continue;
            }

            isCorrectInput = true;
        } while (!isCorrectInput);

        return filePathFromConsole!;
    }
}