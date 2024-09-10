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
                options.UseSqlServer(sqlConfig.ConnectionString));
        }
    }
}
