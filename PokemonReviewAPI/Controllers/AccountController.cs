namespace PokemonReviewAPI.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PokemonReviewAPI.Contract;
    using PokemonReviewAPI.DTO;
    using PokemonReviewAPI.Models;

    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> RegisterAsync(
               [FromBody] UserRegistrationDto requestDto)
        {
            if (requestDto == null)
            {
                return BadRequest("Invalid request payload");
            }

            var authResult = await _accountService.RegisterUserAsync(requestDto);
            return Ok(authResult);
        }

    }
}
