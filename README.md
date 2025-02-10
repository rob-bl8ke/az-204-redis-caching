# Redis Cache

- [Original Source](https://learn.microsoft.com/en-us/training/modules/develop-for-azure-cache-for-redis/4-interact-redis-api)
- [Official Documentation](https://stackexchange.github.io/StackExchange.Redis/)
- [GitHub Project](https://github.com/StackExchange/StackExchange.Redis)
- [Create in Portal](https://learn.microsoft.com/en-us/azure/azure-cache-for-redis/quickstart-create-redis)
- [Redis Azure Documentation](https://learn.microsoft.com/en-us/azure/azure-cache-for-redis/)
- [ASP.NET Core App](https://learn.microsoft.com/en-us/azure/azure-cache-for-redis/cache-web-app-aspnet-core-howto)
- [Quickstart: Use Azure Cache for Redis in .NET Core](https://learn.microsoft.com/en-us/azure/azure-cache-for-redis/cache-dotnet-core-quickstart)



## Add the Package

```
dotnet new console -o Rediscache
dotnet add package StackExchange.Redis
```

## Example Usage

```csharp
using (var cache = ConnectionMultiplexer.Connect(connectionString))
{
    IDatabase db = cache.GetDatabase();

    // Snippet below executes a PING to test the server connection
    var result = await db.ExecuteAsync("ping");
    Console.WriteLine($"PING = {result.Type} : {result}");

    // Call StringSetAsync on the IDatabase object to set the key "test:key" to the value "100"
    bool setValue = await db.StringSetAsync("test:key", "100");
    Console.WriteLine($"SET: {setValue}");

    // StringGetAsync retrieves the value for the "test" key
    string getValue = await db.StringGetAsync("test:key");
    Console.WriteLine($"GET: {getValue}");
}
```

## Code Examples

See [this repository](https://github.com/rustd/RedisSamples/) for a number of [code samples](https://github.com/rustd/RedisSamples/tree/master/HelloWorld) including how [clustering](https://github.com/rustd/RedisSamples/blob/master/HelloWorld/Clustering.cs) changes how you interact with Redis. There are a number of examples here that prototype redis lists, tags, and connection options.

