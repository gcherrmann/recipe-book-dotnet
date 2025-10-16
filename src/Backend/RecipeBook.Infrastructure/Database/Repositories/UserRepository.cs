using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeBook.Infrastructure.Database.Repositories
{
    public class UserRepository : Domain.Repositories.User.IUserRepository
    {
        private readonly RecipeBookDbContext _dbContext;
        public UserRepository(RecipeBookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Domain.Entities.User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Domain.Entities.User?> GetByEmail(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email && u.Active);
        }
    }
}
