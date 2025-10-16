using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;
using RecipeBook.Application.UseCases.User.Register;

namespace RecipeBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisterUserJson),StatusCodes.Status201Created)]
        public async Task<IActionResult> Register([FromServices]IRegisterUserUseCase useCase, [FromBody]RequestRegisterUserJson request)
        {

            var result = await useCase.Execute(request);
            return Created(string.Empty, result);
        }
    }
}
