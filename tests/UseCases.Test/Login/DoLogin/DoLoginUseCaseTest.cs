using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using RecipeBook.Application.Cryptography;
using RecipeBook.Application.UseCases.Login.DoLogin;
using RecipeBook.Communication.Requests;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Test.Login.DoLogin
{
    public class DoLoginUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, var password) = UserBuilder.Build();

            var useCase = CreateUseCase(user);

            var result = await useCase.Execute(new RequestLoginJson
            {
                Email = user.Email,
                Password = password
            });

            result.Should().NotBeNull();
            result.Tokens.Should().NotBeNull();
            result.Name.Should().NotBeNullOrWhiteSpace().And.Be("");
            result.Tokens.AccessToken.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Error_Invalid_User()
        {
            var request = RequestLoginJsonBuilder.Build();

            var useCase = CreateUseCase();

            Func<Task> action = async () => await useCase.Execute(request);

            await action.Should().ThrowAsync<InvalidLoginException>()
                .Where(e => e.Message.Equals(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID));
        }

        private static DoLoginUseCase CreateUseCase(RecipeBook.Domain.Entities.User? user = null)
        {
            var passwordEncripter = new PasswordEncripter();
            var userRepositoryBuilder =new UserRepositoryBuilder();
            var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();

            if (user is { })
            {
                userRepositoryBuilder.GetByEmailAndPassword(user);
            }

            return new DoLoginUseCase(userRepositoryBuilder.Build(), accessTokenGenerator, passwordEncripter);
        }
    }
}
