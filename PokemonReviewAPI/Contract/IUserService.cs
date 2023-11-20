namespace PokemonReviewAPI.Contract
{
    using PokemonReviewAPI.Auth;
    using PokemonReviewAPI.Models;
    using System.Security.Claims;

    public interface IUserService
    {
        string GenerateRefreshToken();

        string GenerateJwtToken(AppUser user);

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
