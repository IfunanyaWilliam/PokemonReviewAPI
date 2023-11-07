using PokemonReviewAPI.Models;
using System.Security.Claims;

namespace PokemonReviewAPI.Contract
{
    public interface IUserService
    {
        Task<AppUser> GetAppUserByIdAsync(string id);
        string GenerateRefreshToken();

        Task<bool> UpdateUserRefreshToken(AppUser user, string token);

        string GenerateJwtToken(AppUser user);

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
