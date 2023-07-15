
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

var config = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json").Build();

string connectionString = config.GetValue<string>("ConnectionString") ?? "";
Console.WriteLine(connectionString);

// Options:
// ConfigurationOptions options = new ConfigurationOptions();

using (var redisConnection = ConnectionMultiplexer.Connect(connectionString))
{
    IDatabase database = redisConnection.GetDatabase();

    // Get and set string values...
    bool wasSet = await database.StringSetAsync("favorite:flavor", "i-love-rocky-road");
    await database.KeyExpireAsync("favorite:flavor", TimeSpan.FromSeconds(10));
    Console.WriteLine($"The value was set? {wasSet}");

    string? value = await database.StringGetAsync("favorite:flavor");
    Console.WriteLine($"The value retrieved for favorite:flavor is {value}");


    // Get and set binary values...
    byte[] binaryKey = new byte[20];
    byte[] binaryValue = new byte[100];
    Random random = new Random();

    random.NextBytes(binaryKey);
    random.NextBytes(binaryValue);

    bool binaryWasSet = await database.StringSetAsync(binaryKey, binaryValue);
    Console.WriteLine($"The binary value was set? {binaryWasSet}");

    byte[]? binaryResult = await database.StringGetAsync(binaryKey);
    Console.WriteLine($"The binary value retrieved for {binaryKey} is {binaryValue}");

    bool binaryValExists = await database.KeyExistsAsync(binaryValue);
    Console.WriteLine($"The binary value exists: {binaryValExists}");

    // Rename key
    database.KeyRename("favorite:flavor", "fav_flavor");
    Console.WriteLine($"favorite:flavor renamed to fav_flavor)");

    // Check on TTL
    var ttl = await database.KeyTimeToLiveAsync("fav_flavor");
    Console.WriteLine($"Time to Live (for fav_flavor) is: {ttl}");

    // execute ping command
    RedisResult result = database.Execute("ping");
    Console.WriteLine(result.ToString()); // displays: "PONG"

    // execute - get all clients connected to the cache
    var result2 = await database.ExecuteAsync("client", "list");
    Console.WriteLine($"Type = {result2.Type}\r\nResult = {result2}");

    // A more complicated example
    GameStat stat = new GameStat("Soccer", new DateTime(2019, 7, 16), "Local Game",
        new[] { "Team 1", "Team 2" },
        new[] { ("Team 1", 2), ("Team 1", 2) });

    string serializeValue = Newtonsoft.Json.JsonConvert.SerializeObject(stat);
    Console.WriteLine(serializeValue);
    bool added = database.StringSet("event:1950-world-cup", serializeValue);

    var worldCupResult = database.StringGet("event:1950-world-cup");
    var worldCupStat = Newtonsoft.Json.JsonConvert.DeserializeObject<GameStat>(worldCupResult.ToString() ?? "");
    Console.WriteLine($"The sport is {worldCupStat?.Sport} played on {worldCupStat?.DatePlayed} by {worldCupStat?.Teams[0]} and {worldCupStat?.Teams[1]}");
}

