using AuctionTrackerWorker.Services;
public class MongoServiceFactory : IMongoServiceFactory
{
    private readonly IServiceScopeFactory _scopeFactory;

    public MongoServiceFactory(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public IMongoService CreateScoped()
    {
        var scope = _scopeFactory.CreateScope();
        return scope.ServiceProvider.GetRequiredService<IMongoService>();
    }
}