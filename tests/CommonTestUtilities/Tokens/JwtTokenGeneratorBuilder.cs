using RecipeBook.Infrastructure.Security.Tokens.Access.Generator;
using RecipeBook.Domain.Security.Token;
using RecipeBook.Domain.Security.Tokens;

namespace CommonTestUtilities.Tokens;

public class JwtTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build() => new JwtTokenGenerator(expirationTimeMinutes: 5, signingKey: "tttttttttttttttttttttttttttttttt");
}