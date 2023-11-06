namespace PokemonReviewAPI.Controllers
{
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Auth;
    using DTO;
    using PokemonReviewAPI.Models;
    using PokemonReviewAPI.Contract;

    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAuthenticationService _authenticationServices;

        public AuthenticationController(
            UserManager<AppUser> userManager,
            IConfiguration configuration,
            IAuthenticationService authenticationServices)
        {
            _userManager = userManager;
            _authenticationServices = authenticationServices;
        }


        [Route("login")]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] UserLoginRequestDto loginRequestDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new AuthResult
                {
                    Result = false,
                    Errors = new List<string> { "Invalid Payload: Email and Password are required" }
                });
            }

            var user = await _userManager.FindByEmailAsync(loginRequestDto.Email);
           
            if (user == null)
            {
                return BadRequest(new AuthResult
                {
                    Message = "Login attempt was not successful",
                    Result = false,
                    Errors = new List<string> { "Invalid Payload" }
                });
            }

            var IsValidCredentials = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (!IsValidCredentials)
            {
                return BadRequest(new AuthResult
                {
                    Message = "Login attempt was not successful",
                    Result = false,
                    Errors = new List<string> { "Invalid Credentials" }
                });
            }

            var jwtToken = _authenticationServices.GenerateJwtToken(user);

            return Ok(new AuthResult
            {
                Message = "Login successful",
                Result = true,
               Token = jwtToken,
            });
        }
    }
}
