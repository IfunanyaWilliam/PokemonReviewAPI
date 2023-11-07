using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Contract
{
    public interface IUserService
    {
        Task<AppUser> GetAppUserByIdAsync(string id);
        string GenerateRefreshToken();

        string GenerateJwtToken(AppUser user);

        Task<bool> UpdateUserRefreshToken(AppUser user, string token);
    }
}
