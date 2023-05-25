
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using AuctionTrackerWorker.Services;
using AuctionTrackerWorker.Models;
using System.Text;
using System.Text.Json;
namespace AuctionTrackerWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _config;
    private string RabbitHostName = string.Empty;
    private string RabbitQueue = string.Empty;
    //private readonly MongoService dbService;
    private readonly IMongoServiceFactory _mongoServiceFactory;

    public Worker(ILogger<Worker> logger, IConfiguration config, /*MongoService service,*/ IMongoServiceFactory mongoServiceFactory)
    {
        _logger = logger;
        _config = config;
        //dbService = service;
        RabbitHostName = _config["rabbithostname"];
        RabbitQueue = _config["rabbitqueue"];
        _mongoServiceFactory = mongoServiceFactory;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        var factory = new ConnectionFactory { HostName = RabbitHostName };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: RabbitQueue,
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
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
            catch(Exception ex)
            {
                _logger.LogInformation("kunne ikke logge bud");
            }
        };
        //basicConsume
        channel.BasicConsume(queue: RabbitQueue,
                            autoAck: true,
                            consumer: consumer);

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(5000, stoppingToken);
        }
    }
}
