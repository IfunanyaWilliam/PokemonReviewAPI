using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Contract
{
    public interface ITokenRepository
    {
        Task<RefreshToken> GetRefreshTokenAsync(string userEmail);

        Task<bool> AddRefreshTokenAsync(RefreshToken refreshToken);
    }
}
