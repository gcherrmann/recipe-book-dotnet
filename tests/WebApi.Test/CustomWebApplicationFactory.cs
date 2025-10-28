﻿using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Infrastructure.Database;

namespace WebApi.Test
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private RecipeBook.Domain.Entities.User _user = default!;
        private string _password = string.Empty;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test").ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<RecipeBookDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<RecipeBookDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provider);
                });

                using var scope = services.BuildServiceProvider().CreateScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<RecipeBookDbContext>();

                dbContext.Database.EnsureDeleted();
                StartDatabase(dbContext);

            }
            );
        }

        public string GetEmail() => _user.Email;
        public string GetPassword() => _password;
        public string GetName() => _user.Name;
        public Guid GetUserIdentifier() => _user.UserIdentifier;

        private void StartDatabase(RecipeBookDbContext context)
        {
            (_user, _password) = UserBuilder.Build();

            context.Users.Add(_user);

            context.SaveChanges();
        }
    }
}
