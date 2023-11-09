namespace PokemonReviewAPI.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Auth;
    using DTO;
    using PokemonReviewAPI.Models;
    using PokemonReviewAPI.Contract;
    using Microsoft.AspNetCore.Authorization;
    using Newtonsoft.Json.Linq;
    using System.Security.Claims;

    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserService _userService;
        private readonly ITokenRepository _tokenRepository;

        public AuthenticationController(
            UserManager<AppUser> userManager,
            IUserService userService,
            ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _userService = userService;
            _tokenRepository = tokenRepository;
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
            var refreshToken = await _tokenRepository.GetRefreshTokenAsync(user.Email);

            if (refreshToken.IsActive)
            {
                return Ok(new AuthResult
                {
                    IsAuthorized = true,
                    Message = "Login Successful 1",
                    Token = jwtToken,
                    RefreshToken = refreshToken.Token
                });
            }

            var newRefreshToken = _userService.GenerateRefreshToken();
            var updateUserResult = await _userService.UpdateUserRefreshToken(user, newRefreshToken);
            if(updateUserResult)
            {
                return Ok(new AuthResult
                {
                    IsAuthorized = true,
                    Message = "Login Successful 2",
                    Token = jwtToken,
                    RefreshToken = newRefreshToken
                });
            }

            return Ok(new AuthResult
            {
                IsAuthorized = false,
                Message = "Login attempt failed",
                Errors = new List<string> { "Internal server error" }
            });
        }


        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> RefreshAccessToken(
                [FromBody]TokenDto token)
        {
            if(string.IsNullOrEmpty(token.AccessToken) || string.IsNullOrEmpty(token.RefreshToken))
            {
                return BadRequest(new AuthResult
                {
                    IsAuthorized = false,
                    Message = "Token could not be regenerated",
                    Errors = new List<string> { "Invalid request payload" }
                });
            }

            var principal = _userService.GetPrincipalFromExpiredToken(token.AccessToken);
            var userEmail = principal.FindFirst(ClaimTypes.Email).Value;
            var user = await _userService.GetAppUserAsync(userEmail);

            if(user == null)
                return BadRequest(new AuthResult
                {
                    IsAuthorized = false,
                    Message = "Token could not be regenerated",
                    Errors = new List<string> { "Invalid user request payload" }
                });


            var refreshToken = user.RefreshTokens.FirstOrDefault(x => x.Token == token.RefreshToken);

            if(refreshToken == null || !refreshToken.IsActive)
                return BadRequest(new AuthResult
                {
                    IsAuthorized = false,
                    Message = "Token could not be regenerated",
                    Errors = new List<string> { "Invalid request payload" }
                });

            var newAccessToken = _userService.GenerateJwtToken(user);
            var newRefreshToken = _userService.GenerateRefreshToken();
            var result = await _userService.UpdateUserRefreshToken(user, newRefreshToken);

            if(result)
            {
                return Ok(new AuthResult
                {
                    IsAuthorized = true,
                    Message = "Token successfully regenerated",
                    Token = newAccessToken,
                    RefreshToken = newRefreshToken
                });
            }

            return BadRequest(new AuthResult
            {
                IsAuthorized = false,
                Message = "Token could not be regenerated",
                Errors = new List<string> { "Invalid request payload" }
            });
        }
    }
}
