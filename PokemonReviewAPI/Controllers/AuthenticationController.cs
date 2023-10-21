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

    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(
            UserManager<IdentityUser> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> RegisterAsync(
                [FromBody] UserRegistrationDto requestDto)
        {
            if(ModelState.IsValid)
            {
                var userExist = await _userManager.FindByEmailAsync(requestDto.Email);
                if (userExist != null)
                {
                    return BadRequest(new AuthResult
                    {
                        Result = false,
                        Errors = new List<string> { "Email already exist" }
                    });
                }

                var newUser = new IdentityUser
                {
                    Email = requestDto.Email,
                    UserName = requestDto.Email
                };

                var result = await _userManager.CreateAsync(newUser, requestDto.Password);
                var Errorlist = new List<string>();

                if (!result.Succeeded)
                {
                    result.Errors.ToList().ForEach(error => Errorlist.Add(error.Description));

                    return BadRequest(new AuthResult
                    {
                        Result = false,
                        Errors = Errorlist
                    });
                }
                //"email": "will@abc.com",
                //"password": "string@A123"

                var token = GenerateJwtToken(newUser);

                return Ok(new AuthResult
                {
                    Token = token,
                    Result = true
                });
            }

            return BadRequest(ModelState);
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

            var userExist = await _userManager.FindByEmailAsync(loginRequestDto.Email);

            if (userExist == null)
            {
                return BadRequest(new AuthResult
                {
                    Result = false,
                    Errors = new List<string> { "Invalid Payload" }
                });
            }

            var IsCorrectCredentials = await _userManager.CheckPasswordAsync(userExist, loginRequestDto.Password);

            if (!IsCorrectCredentials)
            {
                return BadRequest(new AuthResult
                {
                    Result = false,
                    Errors = new List<string> { "Invalid Credentials" }
                });
            }

            var jwtToken = GenerateJwtToken(userExist);

            return Ok(new AuthResult
                      {
                        Result = true,
                        Token = jwtToken,
                      });
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("Id", user.Id),
                        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString())
                    }),

                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}
