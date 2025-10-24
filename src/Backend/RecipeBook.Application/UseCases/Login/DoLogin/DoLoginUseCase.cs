using RecipeBook.Application.Cryptography;
using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;
using RecipeBook.Domain.Repositories.User;
using RecipeBook.Domain.Security.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeBook.Application.UseCases.Login.DoLogin
{
    public class DoLoginUseCase : IDoLoginUseCase
    {
        private readonly IUserRepository _repository;
        private readonly PasswordEncripter _passwordEncripter;
        private readonly IAccessTokenGenerator _accessTokenGenerator;

        public DoLoginUseCase(IUserRepository repository, 
IAccessTokenGenerator accessTokenGenerator, PasswordEncripter passwordEncripter)
        {
            _repository = repository;
            _passwordEncripter = passwordEncripter;
            _accessTokenGenerator = accessTokenGenerator;
        }
        public async Task<ResponseRegisterUserJson> Execute(RequestLoginJson request)
        {
            var encryptedPassword = _passwordEncripter.Encrypt(request.Password);

            var user = await _repository.GetByEmailAndPassword(request.Email, encryptedPassword);

            return new ResponseRegisterUserJson
            {
                Name = user.Name,
                Tokens = new ResponseTokensJson
                {
                    AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier)
                }
            };
        }
    }
}
