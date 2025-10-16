using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;


namespace RecipeBook.Application.UseCases.User.Register
{
    public interface IRegisterUserUseCase
    {
        public Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request);
    }
}
