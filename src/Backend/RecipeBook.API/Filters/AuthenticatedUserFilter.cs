using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RecipeBook.Communication.Responses;
using RecipeBook.Domain.Security.Tokens;
using RecipeBook.Exceptions.ExceptionsBase;
using RecipeBook.Exceptions;
using RecipeBook.Domain.Repositories.User;
using RecipeBook.Infrastructure.Database.Repositories;

namespace RecipeBook.API.Filters
{
    public class AuthenticatedUserFilter : IAsyncAuthorizationFilter
    {
        private readonly IAccessTokenValidator _accessTokenValidator;
        private readonly IUserRepository _repository;

        public AuthenticatedUserFilter(IAccessTokenValidator accessTokenValidator, IUserRepository repository)
        {
            _accessTokenValidator = accessTokenValidator;
            _repository = repository;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                var token = TokenOnRequest(context);

                var userIdentifier = _accessTokenValidator.ValidateAndGetUserIdentifier(token);

                var exist = await _repository.ExistActiveUserWithIdentifier(userIdentifier);
                if (!exist)
                {
                    throw new RecipeBookException(ResourceMessagesException.USER_NOT_FOUND);
                }
            }
            catch (SecurityTokenExpiredException e)
            {
                context.Result = new UnauthorizedObjectResult(
                    new ResponseErrorJson("TokenExpired")
                    {
                        TokenIsExpired = true,
                    });
            }
            catch (RecipeBookException e)
            {
                context.Result = new UnauthorizedObjectResult(
                    new ResponseErrorJson(e.Message));
            }
            catch
            {
                context.Result = new UnauthorizedObjectResult(
                    new RecipeBookException(ResourceMessagesException.USER_NOT_FOUND));
            }

        }

        private static string TokenOnRequest(AuthorizationFilterContext context)
        {
            var authentication = context.HttpContext.Request.Headers.Authorization.ToString();
            if (string.IsNullOrWhiteSpace(authentication))
            {
                throw new RecipeBookException(ResourceMessagesException.NO_TOKEN);
            }

            return authentication["Bearer ".Length..].Trim();
        }
    }
}
