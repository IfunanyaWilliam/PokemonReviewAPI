namespace PokemonReviewAPI.Services
{
    using Microsoft.IdentityModel.Tokens;
    using PokemonReviewAPI.Auth;
    using PokemonReviewAPI.Contract;
    using PokemonReviewAPI.Data;
    using PokemonReviewAPI.Models;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;

    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ITokenRepository _tokenRepository;

        public UserService(AppDbContext context,
                           IConfiguration configuration,
                           ITokenRepository tokenRepository)
        {
            _context = context;
            _configuration = configuration;
            _tokenRepository = tokenRepository;
        }

        public async Task<AuthorizationResult> UpdateUserRefreshTokenAsync(AppUser user, string token)
        {
            var refreshToken = new RefreshToken
            {
                Token = token,
                UserEmail = user.Email,
                Created = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(5)
            };

            var tokenResult = await _tokenRepository.AddRefreshTokenAsync(refreshToken);
            if (tokenResult == null)
                return new AuthorizationResult
                {
                    User = user,
                    RefreshToken = null,
                    IsUserModified = false,
                    IsRefreshTokenSaved = false,
                };

            user.RefreshToken = refreshToken.Token;
            user.RefreshTokenEpirationTime = refreshToken.Expires;
            _context.AppUsers.Update(user);
            var result = await _context.SaveChangesAsync();

            if(result > 0)
            {
                return new AuthorizationResult
                {
                    User = user,
                    RefreshToken = refreshToken,
                    IsRefreshTokenSaved = true,
                    IsUserModified = true
                };
            }

            return new AuthorizationResult 
            {  
                User = null, 
                IsUserModified = false, 
                RefreshToken = null,  
                IsRefreshTokenSaved = false, 
            };
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public string GenerateJwtToken(AppUser user)
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

                Expires = DateTime.UtcNow.AddMinutes(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
        {
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);
            var tokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,      
                    ValidateAudience = false,      
                    RequireExpirationTime = false,   
                    ValidateLifetime = false
                };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }
    }
}
