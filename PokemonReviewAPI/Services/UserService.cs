namespace PokemonReviewAPI.Services
{
    using Microsoft.IdentityModel.Tokens;
    using PokemonReviewAPI.Contract;
    using PokemonReviewAPI.Models;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;

    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;

        public UserService(IUserRepository userRepository,
                           IConfiguration configuration,
                           ITokenRepository tokenRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
        }

        public async Task<AppUser> GetAppUserAsync(string userEmail)
        {
            return await _userRepository.GetAppUserAsync(userEmail);
        }

        public async Task<bool> UpdateUserRefreshToken(AppUser user, string token)
        {
            var refreshToken = new RefreshToken
            {
                Token = token,
                UserEmail = user.Email,
                Created = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(5)
            };

            var tokenResult = await _tokenRepository.AddRefreshTokenAsync(refreshToken);
            if (!tokenResult) return false;

            user.RefreshTokens.Add(refreshToken);
            return await _userRepository.UpdateUserAsync(user);
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
