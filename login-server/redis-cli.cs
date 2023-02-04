
using StackExchange.Redis;

public class RedisClient
{
    string _host = "10.10.100.14";
    public void writeDataToRedis(string key, string value) {
        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(_host);
        IDatabase db = redis.GetDatabase();
        db.StringSet(key, value);
    }
    public void Test()
    {
        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(_host);
        // ^^^ store and re-use this!!!

        IDatabase db = redis.GetDatabase();
        string value = "abcdefg";
        db.StringSet("mykey", value);

        value = db.StringGet("mykey2");
        Console.WriteLine(value); // writes: "abcdefg"
    }
}