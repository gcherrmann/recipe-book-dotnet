using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace RecipeBook.Infrastructure
{
    public static class DependencyInjectionExtension
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            AddRepositories(services);
            AddDbContext_MySql(services,configuration);
            //AddDbContext_SqlServer(services, configuration);
        }

        private static void AddDbContext_MySql(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Database.RecipeBookDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("ConnectionMySql");
                var serverVersion = new MySqlServerVersion(new Version(8, 0, 42));
                options.UseMySql(connectionString, serverVersion);
            });
        }

        private static void AddDbContext_SqlServer(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Database.RecipeBookDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("ConnectionSqlServer");
                options.UseSqlServer(connectionString);
            });
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<Domain.Repositories.User.IUserRepository, Database.Repositories.UserRepository>();
        }
    }
}
