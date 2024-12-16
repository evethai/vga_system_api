using Api.Configurations;
using Api.Services;
using StackExchange.Redis;

namespace Api.Installers
{
    public class CacheInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var redisConfig = new RedisConfiguration();
            configuration.GetSection(nameof(RedisConfiguration)).Bind(redisConfig);

            services.AddSingleton(redisConfig);
            if (!redisConfig.Enabled)
            {
                return;
            }

            var options = ConfigurationOptions.Parse(redisConfig.ConnectionString);
            options.AbortOnConnectFail = false;

            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(options));

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConfig.ConnectionString;
            });

            services.AddSingleton<ICacheService, CacheService>();
        }
    }
}
