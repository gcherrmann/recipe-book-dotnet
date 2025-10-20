using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeBook.Domain.Repositories.User
{
    public interface IUserRepository
    {
        public Task Add(Entities.User user);

        public Task<bool> EmailExists(string email);
    }
}
