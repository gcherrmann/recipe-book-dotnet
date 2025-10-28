using Moq;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories
{

    public class UserRepositoryBuilder
    {
        private readonly Mock<IUserRepository> _repository;

        public UserRepositoryBuilder()
        {
            _repository = new Mock<IUserRepository>();
        }
        public IUserRepository Build()
        {

            return _repository.Object;
        }

        public void ExistActiveUserWithEmail(string email)
        {
            _repository.Setup(repository => repository.ExistActiveUserWithEmail(email)).ReturnsAsync(true);
        }

        public void GetByEmailAndPassword(User user)
        {
            _repository.Setup(repository => repository.GetByEmailAndPassword(user.Email, user.Password)).ReturnsAsync(user);
        }

        public UserRepositoryBuilder GetById(User user)
        {
            _repository.Setup(x => x.GetById(user.Id)).ReturnsAsync(user);
            return this;
        }


    }
}
