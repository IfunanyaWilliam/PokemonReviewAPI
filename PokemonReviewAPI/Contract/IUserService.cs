namespace PokemonReviewAPI.Contract
{
    using PokemonReviewAPI.Auth;
    using PokemonReviewAPI.Models;
    using System.Security.Claims;

    public interface IUserService
    {
        Task<AuthorizationResult> UpdateUserRefreshTokenAsync(AppUser user, string token);

        string GenerateRefreshToken();

        string GenerateJwtToken(AppUser user);

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
