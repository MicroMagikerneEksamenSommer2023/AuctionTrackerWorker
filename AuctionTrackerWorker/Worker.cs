using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using AuctionTrackerWorker.Services;
using AuctionTrackerWorker.Models;
using System.Text;
using System.Text.Json;

namespace AuctionTrackerWorker;

public class Worker : BackgroundService
{
    // Attributter
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _config;
    private string RabbitHostName = string.Empty;
    private string RabbitQueue = string.Empty;
    //private readonly MongoService dbService;
    private readonly IMongoServiceFactory _mongoServiceFactory;

    // Constructor
    public Worker(ILogger<Worker> logger, IConfiguration config, /*MongoService service,*/ IMongoServiceFactory mongoServiceFactory)
    {
        _logger = logger;
        _config = config;
        //dbService = service;
        RabbitHostName = _config["rabbithostname"];
        RabbitQueue = _config["rabbitqueue"];
        _mongoServiceFactory = mongoServiceFactory;
    }

    // Metoden køres som en baggrundsopgave, der lytter efter beskeder i RabbitMQ-køen.
    // Når der modtages en besked, parses den som et budobjekt og logges ved hjælp af MongoDB-service.
    // Hvis der opstår en fejl under loggingen, logges en passende fejlbesked.
    // Metoden fortsætter med at køre i en løkke, der afbrydes, når afbrydelsesforespørgslen modtages.
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Opretter en forbindelsesfabrik og opretter en forbindelse og kanal til RabbitMQ
        var factory = new ConnectionFactory { HostName = RabbitHostName };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: RabbitQueue, // Definerer køen, hvor beskederne skal modtages fra
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
    
        var consumer = new EventingBasicConsumer(channel); // Opretter en forbruger, der skal modtage beskeder fra køen
        consumer.Received += async (model, ea) => // Definerer handlingen, der skal udføres, når der modtages en besked
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Bid newBid = JsonSerializer.Deserialize<Bid>(message);
            try
            {
                var mongoService = _mongoServiceFactory.CreateScoped();
                await mongoService.LogBid(newBid);
                _logger.LogInformation("updated existing item");
            }
            catch (Exception ex)
            {
                _logger.LogInformation("kunne ikke logge bud");
            }
        };
        //basicConsume
        channel.BasicConsume(queue: RabbitQueue, // Starter forbrugeren, der lytter efter beskeder i køen og udfører den angivne handling
                            autoAck: true,
                            consumer: consumer);

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(5000, stoppingToken); // Venter i 5 sekunder, inden løkken bliver gentaget
        }
    }
}
