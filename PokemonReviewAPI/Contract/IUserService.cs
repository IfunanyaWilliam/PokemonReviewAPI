using PokemonReviewAPI.Models;
using System.Security.Claims;

namespace PokemonReviewAPI.Contract
{
    public interface IUserService
    {
        Task<AppUser> GetAppUserAsync(string userEmail);

        Task<bool> UpdateUserRefreshToken(AppUser user, string token);

        string GenerateRefreshToken();

        string GenerateJwtToken(AppUser user);

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
