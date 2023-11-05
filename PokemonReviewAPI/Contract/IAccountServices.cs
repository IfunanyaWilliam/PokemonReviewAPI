using PokemonReviewAPI.Auth;
using PokemonReviewAPI.DTO;

namespace PokemonReviewAPI.Contract
{
    public interface IAccountServices
    {
        Task<AuthResult> RegisterUserAsync(UserRegistrationDto requestDto);
    }
}
