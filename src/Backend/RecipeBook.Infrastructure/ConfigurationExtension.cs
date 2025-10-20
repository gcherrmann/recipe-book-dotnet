using Microsoft.Extensions.Configuration;

namespace RecipeBook.Infrastructure
{
    public static class ConfigurationExtension
    {
        public static bool IsTestEnvironment(this IConfiguration configuration)
        {
            return configuration.GetValue<bool>("InMemoryTest");
        }
    }
}
