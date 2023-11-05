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
        private readonly UserManager<AppUser> _userManager;
        private readonly IAccountServices _accountServices;

        public AccountController(UserManager<AppUser> userManager,
                                 IAccountServices accountServices)
        {
             _userManager = userManager;
            _accountServices = accountServices;
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

            var authResult = await _accountServices.RegisterUserAsync(requestDto);
            return new JsonResult(authResult);
        }

    }
}
