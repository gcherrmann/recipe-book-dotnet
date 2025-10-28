using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using RecipeBook.Application.UseCases.User.ChangePassword;
using RecipeBook.Communication.Requests;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;

namespace UseCases.Test.User.ChangePassword
{
    public class ChangePasswordUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, var password) = UserBuilder.Build();

            var request = RequestChangePasswordJsonBuilder.Build();
            request.Password = password;

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => await useCase.Execute(request);

            await act.Should().NotThrowAsync();

            var passwordEncripter = PasswordEncripterBuilder.Build();

            user.Password.Should().Be(passwordEncripter.Encrypt(request.NewPassword));
        }

        [Fact]
        public async Task Error_NewPassword_Empty()
        {
            (var user, var password) = UserBuilder.Build();

            var request = new RequestChangePasswordJson
            {
                Password = password,
                NewPassword = string.Empty
            };
            var useCase = CreateUseCase(user);

            Func<Task> act = async () => { await useCase.Execute(request); };

            (await act.Should().ThrowAsync<ErrorOnValidationException>())
                .Where(e => e.Errors.Count == 1 &&
                    e.Errors.Contains(ResourceMessagesException.PASSWORD_EMPTY));

            var passwordEncripter = PasswordEncripterBuilder.Build();

            user.Password.Should().Be(passwordEncripter.Encrypt(password));
        }

        [Fact]
        public async Task Error_CurrentPassword_Different()
        {
            (var user, var password) = UserBuilder.Build();

            var request = RequestChangePasswordJsonBuilder.Build();

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => { await useCase.Execute(request); };

            await act.Should().ThrowAsync<ErrorOnValidationException>()
                .Where(e => e.Errors.Count == 1 &&
                    e.Errors.Contains(ResourceMessagesException.PASSWORD_DIFFERENT_CURRENT_PASSWORD));

            var passwordEncripter = PasswordEncripterBuilder.Build();

            user.Password.Should().Be(passwordEncripter.Encrypt(password));
        }

        private static ChangePasswordUseCase CreateUseCase(RecipeBook.Domain.Entities.User user)
        {
            var userUpdateRepository = new UserRepositoryBuilder().GetById(user).Build();
            var loggedUser = LoggedUserBuilder.Build(user);
            var passwordEncripter = PasswordEncripterBuilder.Build();

            return new ChangePasswordUseCase(loggedUser, passwordEncripter, userUpdateRepository);
        }
    }
}
