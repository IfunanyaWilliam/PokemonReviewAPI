using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Contract
{
    public interface IAuthenticationService
    {
        string GenerateRefreshToken();

        string GenerateJwtToken(AppUser user);
    }
}
