using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Contract
{
    public interface ITokenRepository
    {
        Task<RefreshToken> GetRefreshTokenAsync(string token);
    }
}
