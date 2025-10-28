namespace RecipeBook.Domain.Repositories.User
{
    public interface IUserRepository
    {
        public Task<bool> ExistActiveUserWithEmail(string email);
        public Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier);
        public Task<Entities.User?> GetByEmailAndPassword(string email, string password);
        public Task<Entities.User> GetById(long id);
        public Task Update(Entities.User user);
        public Task Add(Entities.User user);

    }
}
