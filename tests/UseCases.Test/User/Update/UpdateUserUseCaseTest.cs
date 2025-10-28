using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using RecipeBook.Application.UseCases.User.Update;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;

namespace UseCases.Test.User.Update
{
    public class UpdateUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var request = RequestUpdateUserJsonBuilder.Build();

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => await useCase.Execute(request);

            await act.Should().NotThrowAsync();

            user.Name.Should().Be(request.Name);
            user.Email.Should().Be(request.Email);
        }

        [Fact]
        public async Task Error_Name_Empty()
        {
            (var user, _) = UserBuilder.Build();

            var request = RequestUpdateUserJsonBuilder.Build();
            request.Name = string.Empty;

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => { await useCase.Execute(request); };

            (await act.Should().ThrowAsync<ErrorOnValidationException>())
                .Where(e => e.Errors.Count == 1 &&
                    e.Errors.Contains(ResourceMessagesException.NAME_EMPTY));

            user.Name.Should().NotBe(request.Name);
            user.Email.Should().NotBe(request.Email);
        }

        [Fact]
        public async Task Error_Email_Already_Registered()
        {
            (var user, _) = UserBuilder.Build();

            var request = RequestUpdateUserJsonBuilder.Build();

            var useCase = CreateUseCase(user, request.Email);

            Func<Task> act = async () => { await useCase.Execute(request); };

            await act.Should().ThrowAsync<ErrorOnValidationException>()
                .Where(e => e.Errors.Count == 1 &&
                    e.Errors.Contains(ResourceMessagesException.EMAIL_ALREADY_REGISTERED));

            user.Name.Should().NotBe(request.Name);
            user.Email.Should().NotBe(request.Email);
        }

        private static UpdateUserUseCase CreateUseCase(RecipeBook.Domain.Entities.User user, string? email = null)
        {
            var userUpdateRepository = new UserRepositoryBuilder().GetById(user).Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            var userRepositoryBuilder = new UserRepositoryBuilder();
            if (!string.IsNullOrEmpty(email))
                userRepositoryBuilder.ExistActiveUserWithEmail(email);

            return new UpdateUserUseCase(loggedUser, userUpdateRepository);
        }
    }
}
