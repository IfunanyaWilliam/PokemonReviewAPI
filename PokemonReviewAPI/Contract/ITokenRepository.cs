using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Contract
{
    public interface ITokenRepository
    {
        Task<RefreshToken> GetRefreshTokenAsync(string userEmail);

        Task<RefreshToken> AddRefreshTokenAsync(RefreshToken refreshToken);

        Task<bool> UpdateRefreshTokenAsync(RefreshToken refreshToken);

        Task<bool> RevokeRefreshTokenAsync(string token);
    }
}
