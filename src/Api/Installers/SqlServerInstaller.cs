using Api.Configurations;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Installers
{
    public class SqlServerInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var sqlConfig = new SqlServerConfiguration();
            configuration.GetSection(nameof(SqlServerConfiguration)).Bind(sqlConfig);
            services.AddSingleton(sqlConfig);
            services.AddDbContext<VgaDbContext>(options =>
                options.UseSqlServer(sqlConfig.ConnectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null
                    );
                }));

            //services.AddDbContext<VgaDbContext>(options =>
            //    options.UseNpgsql(sqlConfig.ConnectionString, npgsqlOptions =>
            //    {
            //        npgsqlOptions.EnableRetryOnFailure(
            //            maxRetryCount: 5,
            //            maxRetryDelay: TimeSpan.FromSeconds(10),
            //            errorCodesToAdd: null
            //        );
            //    }));
        }
    }
}
