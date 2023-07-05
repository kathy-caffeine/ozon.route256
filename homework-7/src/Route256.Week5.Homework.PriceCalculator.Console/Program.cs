// See https://aka.ms/new-console-template for more information
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Route256.Week5.Homework.PriceCalculator.Grpc;

Console.WriteLine("Hello, World!");

var host = new HostBuilder()
    .ConfigureServices(services =>
    services.AddGrpcClient<PriceCalculator.PriceCalculatorClient>(
        o =>
        {
            o.Address = new Uri("http://localhost:5207");
        })
    )
    .Build();

await host.StartAsync();
await RunAsync(host.Services.GetRequiredService<PriceCalculator.PriceCalculatorClient>());
await host.StopAsync();

async Task RunAsync(PriceCalculator.PriceCalculatorClient client)
{
    Console.WriteLine("Введите название метода:\r\n" +
    "1. Calculate\r\n" +
    "2. Clear History\r\n" +
    "3. Get History");

    var methodId = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine("Введите id пользователя:");
    var userId = Convert.ToInt64(Console.ReadLine());

    switch (methodId)
    {
        case 1:
            {
                Console.WriteLine("Введите количество товаров");
                var amount = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Вводите параметры товаров в формате\n" +
                        "<Длина> <Ширина> <Высота> <Вес>");
                var goods = new GoodEntity[amount];
                for (int i = 0; i < goods.Length; i++)
                {
                    var str = Console.ReadLine()!.Split(" ");
                    if (str == null) continue;
                    goods[i] = new GoodEntity()
                    {
                        Length = Convert.ToDouble(str[0]),
                        Width = Convert.ToDouble(str[1]),
                        Height = Convert.ToDouble(str[2]),
                        Weight = Convert.ToDouble(str[3])
                    };
                }

                CountPrice(new DeliveryPriceRequest()
                {
                    UserId = userId,
                    Goods = { goods }
                }, client);
                break;
            }
        case 2:
            {
                Console.WriteLine("Введите количество товаров");
                var amount = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Введите идентификаторы товаров, данные о которых хотите удалить");
                var ids = new long[amount];
                for (int i = 0; i < amount; i++)
                {
                    ids[i] = Convert.ToInt64(Console.ReadLine()!);
                }

                DeleteRecords(new DeleteUserHistoryRequest()
                {
                    UserId = userId,
                    GoodIds = { ids }
                }, client);
                break;
            }
        case 3:
            {
                await PrintHistory(new GetUserHistoryRequest()
                {
                    UserId = userId
                }, client);
                break;
            }
    }
}

void CountPrice(DeliveryPriceRequest request,
    PriceCalculator.PriceCalculatorClient client)
{
    var call = client.CountDeliveryPrice(request);
    Console.WriteLine("Результаты вычислений:");
    Console.WriteLine(call.Result);
}

void DeleteRecords(DeleteUserHistoryRequest request, 
    PriceCalculator.PriceCalculatorClient client)
{
    var call_2 = client.DeleteUserHistory(request);
    if (call_2 != null) Console.WriteLine("Записи удалены.");
}

async Task PrintHistory(GetUserHistoryRequest request, 
    PriceCalculator.PriceCalculatorClient client)
{
    var call_3 = client.GetUserHistory(request);

    var historyResponse = Task.Run(async () =>
    {
        await foreach (var response in call_3.ResponseStream.ReadAllAsync())
        {
            Console.WriteLine("ID товара: " + response.GoodId);
        }

    });
    await historyResponse;
}