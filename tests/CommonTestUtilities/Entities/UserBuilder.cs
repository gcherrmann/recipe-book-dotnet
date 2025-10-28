using Bogus;
using RecipeBook.Application.Cryptography;
using RecipeBook.Domain.Entities;

namespace CommonTestUtilities.Entities
{
    public class UserBuilder
    {
        public static (User user, string password) Build()
        {
            var passwordEncrypter = new PasswordEncripter();

            var password = new Faker().Internet.Password();

            var user = new Faker<User>()
                .RuleFor(u => u.Id, () => 1)
                .RuleFor(u => u.Name, f => f.Person.FirstName)
                .RuleFor(u => u.Email, (f, user) => f.Internet.Email(user.Name))
                .RuleFor(user => user.UserIdentifier, _ => Guid.NewGuid())
                .RuleFor(u => u.Password, f => passwordEncrypter.Encrypt(password));

            return (user, password);

        }
    }
}
