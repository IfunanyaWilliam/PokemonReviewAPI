namespace PokemonReviewAPI.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Auth;
    using DTO;
    using PokemonReviewAPI.Models;
    using PokemonReviewAPI.Contract;

    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserService _userService;

        public AuthenticationController(
            UserManager<AppUser> userManager,
            IUserService authenticationService)
        {
            _userManager = userManager;
            _userService = authenticationService;
        }


        [Route("login")]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] UserLoginRequestDto loginRequestDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new AuthResult
                {
                    IsAuthorized = false,
                    Message = "Login attempt failed",
                    Errors = new List<string> { "Invalid Payload: Email and Password are required" }
                });
            }

            var user = await _userManager.FindByEmailAsync(loginRequestDto.Email);
           
            if (user == null)
            {
                return BadRequest(new AuthResult
                {
                    IsAuthorized = false,
                    Message = "Login attempt failed",
                    Errors = new List<string> { "Invalid Payload" }
                });
            }

            var IsValidCredentials = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (!IsValidCredentials)
            {
                return BadRequest(new AuthResult
                { 
                    IsAuthorized = false,
                    Message = "Login attempt failed",
                    Errors = new List<string> { "Invalid Credentials" }
                });
            }
            
            var jwtToken = _userService.GenerateJwtToken(user);

            var currentUser = await _userService.GetAppUserByIdAsync(user.Id);

            if (currentUser.RefreshTokens.Any(x => x.IsActive))
            {
                return Ok(new AuthResult
                {
                    IsAuthorized = true,
                    Message = "Login Successful 1",
                    Token = jwtToken,
                    RefreshToken = user.RefreshTokens.FirstOrDefault(a => a.IsActive).Token
                });
            }

            var refreshToken = _userService.GenerateRefreshToken();
            var updateUserResult = await _userService.UpdateUserRefreshToken(user, refreshToken);
            if(updateUserResult)
            {
                return Ok(new AuthResult
                {
                    IsAuthorized = true,
                    Message = "Login Successful 2",
                    Token = jwtToken,
                    RefreshToken = refreshToken
                });
            }


            return Ok(new AuthResult
            {
                IsAuthorized = false,
                Message = "Login attempt failed",
                Errors = new List<string> { "Internal server error" }
            });
        }
    }
}
