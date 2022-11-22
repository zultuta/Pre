using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pre.UserProjectManager.Core.Interfaces.Infrastructure;
using Pre.UserProjectManager.Infrastructure.Data;
using Pre.UserProjectManager.Infrastructure.Data.DAL;

namespace Pre.UserProjectManager.Infrastructure.Extension
{
    public static class IServiceCollectionExtension
    {
        //public static void ConfigureDBContext(this IServiceCollection services, string connectionString)
        //{
        //    services.AddDbContext<ProjectManagerDbContext>(options =>
        //    {
        //        options.UseSqlServer(connectionString);
        //    });
        //}

        static void ConfigureDBContextPool(this IServiceCollection services, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            services.AddDbContextPool<ProjectManagerDbContext>(optionBuilder => ConfigureDbContextOptionBuilder(optionBuilder, connectionString));
        }

        static void ConfigureDbContextOptionBuilder(DbContextOptionsBuilder optionBuilder, string connectionString)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 25));

            optionBuilder
               .EnableDetailedErrors()
               .UseMySql(connectionString, serverVersion, serverOptions =>
               {
                   serverOptions.EnableRetryOnFailure();
                   serverOptions.CommandTimeout((int)TimeSpan.FromMinutes(2).TotalSeconds);
               });
        }

        public static void ResolveInfrastructureServices(this IServiceCollection services, string connectionString)
        {
            services.ConfigureDBContextPool(connectionString);
            services.AddScoped<IProjectManagerQuery, ProjectManagerQuery>();
            services.AddScoped<IProjectManagerCommand, ProjectManagerCommand>();
        }
    }
}
