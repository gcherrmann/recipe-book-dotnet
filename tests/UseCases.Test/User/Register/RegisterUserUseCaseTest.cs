using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using RecipeBook.Application.UseCases.User.Register;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Test.User.Register
{
    public class RegisterUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {

            var request = RequestRegisterUserJsonBuilder.Build();

            var useCase = CreateUseCase();
            var result = await useCase.Execute(request);

            result.Should().NotBeNull();
            result.Tokens.Should().NotBeNull();
            result.Name.Should().Be(request.Name);
            result.Tokens.AccessToken.Should().NotBeNullOrWhiteSpace();

        }

        [Fact]
        public async Task Email_Already_Exists()
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            var useCase = CreateUseCase(request.Email);
            Func<Task> action = async () => await useCase.Execute(request);

            (await action.Should().ThrowAsync<ErrorOnValidationException>())
                .Where(e => e.Errors.Count == 1 && e.Errors.Contains(ResourceMessagesException.EMAIL_ALREADY_REGISTERED));

        }

        [Fact]
        public async Task Name_Empty()
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            var useCase = CreateUseCase();
            Func<Task> action = async () => await useCase.Execute(request);

            (await action.Should().ThrowAsync<ErrorOnValidationException>())
                .Where(e => e.Errors.Count == 1 && e.Errors.Contains(ResourceMessagesException.NAME_EMPTY));

        }

        private RegisterUserUseCase CreateUseCase(string email = null)
        {
            
            var mapper = MapperBuilder.Builder();
            var passwordEncripter = PasswordEncrypterBuilder.Build();
            var userRepository = new UserRepositoryBuilder().Build();
            var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();


            return new RegisterUserUseCase(userRepository, mapper, passwordEncripter, accessTokenGenerator);
        }
    }
}
