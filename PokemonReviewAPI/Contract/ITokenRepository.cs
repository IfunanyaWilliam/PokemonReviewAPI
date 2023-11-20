using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Contract
{
    public interface ITokenRepository
    {
        Task<RefreshToken> GetRefreshTokenAsync(string userEmail);

        Task<RefreshToken> AddRefreshTokenAsync(string refreshToken, AppUser user);

        Task<bool> UpdateRefreshTokenAsync(RefreshToken refreshToken);

        Task<bool> RevokeRefreshTokenAsync(string token);

        Task DeleteExpiredRefreshTokenAsync();
    }
}
