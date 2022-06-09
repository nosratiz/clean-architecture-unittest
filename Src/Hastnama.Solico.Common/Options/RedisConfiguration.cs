namespace Hastnama.Solico.Common.Options
{
    public interface IRedisConfiguration
    {
        string Connection { get; set; }

        string InstanceName { get; set; }
    }

    public class RedisConfiguration : IRedisConfiguration
    {
        public string Connection { get; set; }
        public string InstanceName { get; set; }
    }
}