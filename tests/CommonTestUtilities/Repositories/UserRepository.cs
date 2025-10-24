using Moq;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTestUtilities.Repositories
{
    
    public class UserRepositoryBuilder
    {
        private readonly Mock<IUserRepository> _repository;

        public UserRepositoryBuilder()
        {
            _repository = new Mock<IUserRepository>();
        }
        public IUserRepository Build() {

            return _repository.Object;
        }

        public void ExistActiveUserWithEmail(string email)
        {
            _repository.Setup(r=> r.EmailExists(email)).ReturnsAsync(true);
        }

        public void GetByEmailAndPassword(User user)
        {
            _repository.Setup(r => r.GetByEmailAndPassword(user.Email, user.Password)).ReturnsAsync(user);
        }
    }
}
