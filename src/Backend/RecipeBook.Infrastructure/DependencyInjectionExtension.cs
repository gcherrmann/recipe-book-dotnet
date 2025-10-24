using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Domain.Security.Tokens;
using RecipeBook.Infrastructure.Security.Tokens.Access.Generator;
using System.Reflection;


namespace RecipeBook.Infrastructure
{
    public static class DependencyInjectionExtension
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            AddRepositories(services);
            AddTokens(services, configuration);

            if (configuration.IsTestEnvironment())
            {
                return;
            }

            AddDbContext_MySql(services,configuration);
            AddFluentMigrator_MySql(services, configuration);
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

        private static void AddTokens(IServiceCollection services, IConfiguration configuration)
        {
            var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
            var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

            services.AddScoped<IAccessTokenGenerator>(option => new JwtTokenGenerator(expirationTimeMinutes, signingKey!));
            //services.AddScoped<IAccessTokenValidator>(option => new JwtTokenValidator(signingKey!));

            //services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
        }

        private static void AddFluentMigrator_MySql(IServiceCollection services, IConfiguration configuration) 
        {
            services.AddFluentMigratorCore().ConfigureRunner(
                options=> options.AddMySql5()
                .WithGlobalConnectionString(configuration.GetConnectionString("ConnectionMySql"))
                    .ScanIn(Assembly.Load("RecipeBook.Infrastructure")).For.All()
                );
                
        }

        private static void AddFluentMigrator_SqlServer(IServiceCollection services, IConfiguration configuration) 
        {
            services.AddFluentMigratorCore().ConfigureRunner(
                options => options.AddSqlServer()
                .WithGlobalConnectionString(configuration.GetConnectionString("ConnectionSqlServer"))
                    .ScanIn(Assembly.Load("RecipeBook.Infrastructure")).For.All()
                );
        }
    }
}
